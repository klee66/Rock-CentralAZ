// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.IO;
using System.Linq;
using System.Web;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Security;

namespace RockWeb
{
    /// <summary>
    /// Handles retrieving file data from storage
    /// </summary>
    public class GetFile : IHttpAsyncHandler
    {
        /// <summary>
        /// Called to initialize an asynchronous call to the HTTP handler. 
        /// </summary>
        /// <param name="context">An HttpContext that provides references to intrinsic server objects used to service HTTP requests.</param>
        /// <param name="cb">The AsyncCallback to call when the asynchronous method call is complete.</param>
        /// <param name="extraData">Any state data needed to process the request.</param>
        /// <returns>An IAsyncResult that contains information about the status of the process.</returns>
        public IAsyncResult BeginProcessRequest( HttpContext context, AsyncCallback cb, object extraData )
        {
            try
            {
                // Check to see if this is a BinaryFileType/BinaryFile or just a plain content file (if isBinaryFile not specified, assume it is a BinaryFile)
                bool isBinaryFile = ( context.Request.QueryString["isBinaryFile"] ?? "T" ).AsBoolean();
                context.Items.Add( "isBinaryFile", isBinaryFile );

                if ( isBinaryFile )
                {
                    return BeginProcessBinaryFileRequest( context, cb );
                }
                else
                {
                    return BeginProcessContentFileRequest( context );
                }
            }
            catch ( Exception ex )
            {
                ExceptionLogService.LogException( ex, context );
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = ex.Message;
                context.ApplicationInstance.CompleteRequest();

                return null;
            }
        }

        /// <summary>
        /// Begins the process content file request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">fileName must be specified</exception>
        private static IAsyncResult BeginProcessContentFileRequest( HttpContext context )
        {
            string relativeFilePath = context.Request.QueryString["fileName"];

            if ( string.IsNullOrWhiteSpace( relativeFilePath ) )
            {
                throw new Exception( "fileName must be specified" );
            }

            const string RootContentFolder = "~/Content";
            string physicalRootFolder = context.Request.MapPath( RootContentFolder );
            string physicalContentFileName = Path.Combine( physicalRootFolder, relativeFilePath.TrimStart( new char[] { '/', '\\' } ) );
            byte[] fileContents;

            using ( FileStream sourceStream = File.Open( physicalContentFileName, FileMode.Open, FileAccess.Read ) )
            {
                fileContents = new byte[sourceStream.Length];
                context.Items.Add( "fileContents", fileContents );
                context.Items.Add( "physicalContentFileName", physicalContentFileName );
                return sourceStream.ReadAsync( fileContents, 0, (int)sourceStream.Length );
            }
        }

        /// <summary>
        /// Begins the process binary file request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cb">The cb.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">file id key must be a guid or an int</exception>
        private static IAsyncResult BeginProcessBinaryFileRequest( HttpContext context, AsyncCallback cb )
        {
            int fileId = context.Request.QueryString["id"].AsInteger() ?? 0;
            Guid fileGuid = context.Request.QueryString["guid"].AsGuid();

            if ( fileId == 0 && fileGuid.Equals( Guid.Empty ) )
            {
                throw new Exception( "file id key must be a guid or an int" );
            }

            BinaryFileService binaryFileService = new BinaryFileService( new RockContext() );
            if ( fileGuid != Guid.Empty )
            {
                return binaryFileService.BeginGet( cb, context, fileGuid );
            }
            else
            {
                return binaryFileService.BeginGet( cb, context, fileId );
            }
        }

        /// <summary>
        /// Provides an end method for an asynchronous process. 
        /// </summary>
        /// <param name="result">An IAsyncResult that contains information about the status of the process.</param>
        public void EndProcessRequest( IAsyncResult result )
        {
            // restore the context from the asyncResult.AsyncState 
            HttpContext context = (HttpContext)result.AsyncState;

            try
            {
                context.Response.Clear();

                bool isBinaryFile = (bool)context.Items["isBinaryFile"];

                if ( isBinaryFile )
                {
                    var rockContext = new RockContext();

                    bool requiresSecurity = false;
                    BinaryFile binaryFile = new BinaryFileService( rockContext ).EndGet( result, context, out requiresSecurity );
                    if ( binaryFile != null )
                    {
                        //// if the binaryFile's BinaryFileType requires security, check security
                        //// note: we put a RequiresSecurity flag on BinaryFileType because checking security for every file would be slow (~40ms+ per request)
                        if ( requiresSecurity )
                        {
                            var currentUser = new UserLoginService( rockContext ).GetByUserName( UserLogin.GetCurrentUserName() );
                            Person currentPerson = currentUser != null ? currentUser.Person : null;
                            binaryFile.BinaryFileType = binaryFile.BinaryFileType ?? new BinaryFileTypeService( rockContext ).Get( binaryFile.BinaryFileTypeId.Value );
                            if ( !binaryFile.IsAuthorized( Authorization.VIEW, currentPerson ) )
                            {
                                SendNotAuthorized( context );
                                return;
                            }
                        }
                        
                        context.Response.AddHeader( "content-disposition", string.Format( "inline;filename={0}", binaryFile.FileName ) );
                        context.Response.ContentType = binaryFile.MimeType;

                        if ( binaryFile.Data != null )
                        {
                            if ( binaryFile.Data.Content != null )
                            {
                                context.Response.BinaryWrite( binaryFile.Data.Content );
                                context.Response.Flush();
                                context.ApplicationInstance.CompleteRequest();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    byte[] fileContents = (byte[])context.Items["fileContents"];
                    string physicalContentFileName = context.Items["physicalContentFileName"] as string;

                    if ( fileContents != null )
                    {
                        context.Response.AddHeader( "content-disposition", string.Format( "inline;filename={0}", Path.GetFileName( physicalContentFileName ) ) );

                        string mimeType = System.Web.MimeMapping.GetMimeMapping( physicalContentFileName );
                        context.Response.ContentType = mimeType;

                        context.Response.BinaryWrite( fileContents );
                        context.Response.Flush();
                        context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                }

                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "Unable to find the requested file.";
            }
            catch ( Exception ex )
            {
                ExceptionLogService.LogException( ex, context );
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = ex.Message;
                context.Response.Flush();
                context.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ProcessRequest( HttpContext context )
        {
            throw new NotImplementedException( "The method or operation is not implemented. This is an asynchronous file handler." );
        }

        /// <summary>
        /// Sends a 403 (forbidden)
        /// </summary>
        /// <param name="context">The context.</param>
        private void SendNotAuthorized( HttpContext context )
        {
            context.Response.StatusCode = System.Net.HttpStatusCode.Forbidden.ConvertToInt();
            context.Response.StatusDescription = "Not authorized to view file";
            context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
//
//  ZebraPrint.js
//  Zebra Print plugin for Cordova iOS
//
//  Copyright 2013 Spark Development Network. 
//


var ZebraPrintPlugin = {
    
    // print tags
    printTags: function (tagJson, success, fail)
    {
        Cordova.exec(success, fail, "ZebraPrint", "printTags", [tagJson]);
    }
};

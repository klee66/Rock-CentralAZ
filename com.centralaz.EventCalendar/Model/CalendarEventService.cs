﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.centralaz.EventCalendar.Data;
using Rock.Model;

namespace com.centralaz.EventCalendar.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class CalendarEventService : EventCalendarService<CalendarEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEventService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CalendarEventService( EventCalendarContext context ) : base( context ) { }

        public List<CalendarEvent> GetEvents(List<int> campusList){
            return Queryable()
                .Where( ce => campusList.Any( cl => cl == ce.CampusId ) || !ce.CampusId.HasValue )
                .ToList();
        }

        public List<CalendarEvent> GetEvents()
        {
            return Queryable().ToList();
        }


        
    }
}
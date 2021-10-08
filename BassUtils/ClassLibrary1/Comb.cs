﻿/*
 * COMBs - A class for generating sequentially ordered GUIDs (to avoid index fragmentation in SQL Server).
 * 
 * The GUID returned is basically a GUID where the MAC address has been replaced by an encoding
 * of the current time. It is these last 6 bytes which SQL server uses to perform the ordering, obviously
 * because time is always increasing (assuming the time is right on your COMB generating machines)
 * this causes new rows to be inserted at the end of the clustered index, thus avoiding fragmentation.
 * 
 * COMBs were invented by Jimmy Nilsson in this widely-quoted article:
 *     http://www.informit.com/articles/article.aspx?p=25862&seqNum=1&rl=1
 * 
 *  The code below is a direct lift of his implementation found here:
 *     http://www.jnsk.se/informit/pkguid/followup.htm
 * 
 * No performance tweaking has been done. On a Pentium D 3.4GHz machine this 
 * simple algorithm generates 100,000 COMBs in 0.2 seconds, which is far faster than
 * you can insert rows into SQL server.
 * 
 * 
 * 
 * MULTIPLE CLIENTS and COMPATIBILITY WITH SQL SERVER's NEWSEQUENTIALID()
 * 
 * The GUIDs generated by this algorithm will order sequentially regardless of which
 * machine they are generated on. This is because they have overwritten the MAC address
 * with the current time. This is an extremely important consideration if you are generating
 * GUIDs outside the database (say in a data access layer or business logic layer) on 
 * multiple machines. Other algorithms which include the MAC address (such as the yafla one
 * referenced below) do not have this behaviour and would lead to index fragmentation if
 * used on more than one machine.
 * 
 * The caveat to this is that SQL Server's NewSequentialId() function is NOT compatible
 * with these GUIDs because it *does* include the MAC address. This means that if you
 * use NewSequentialId() for some of your IDs and COMBs for the others you will still get
 * a degree of index fragmentation. The solution is to install the COMB code as a SQL CLR
 * function and call it from T-SQL whenever you need to generate a new GUID in the database.
 * 
 * On SQL2000, an extended stored procedure would be required.
 * 
 * FURTHER READING
 * 
 * The Microsoft SQL team's blog entry describing NewSequentialId():
 * http://blogs.msdn.com/sqlprogrammability/archive/2006/03/23/559061.aspx
 * 
 * Kimberly Tripp's blog entry describing her criteria for clustered indices:
 * http://www.sqlskills.com/blogs/kimberly/PermaLink.aspx?guid=bdaee3f7-1e15-414b-b75f-a290db645159
 * 
 * Background only, this algorithm is not used here:
 * http://www.yafla.com/dforbes/stories/2005/10/17/yaflaguid.html 
 * 
 */

using System;

namespace ClassLibrary1
{
    /// <summary>
    /// Static class for creating new GUIDs that are monotonically increasing when ordered by SQL Server.
    /// </summary>
    public static class Comb
    {
        /// <summary>
        /// Returns a new Guid using the COMB algorithm.
        /// </summary>
        /// <returns>A new COMB Guid.</returns>
        public static Guid NewGuid()
        {
            DateTime theNow = DateTime.Now;

            // Calculate the days after 1st of Jan 1900.
            DateTime startOfCentury = new DateTime(1900, 1, 1);
            Int32 theDays = theNow.Subtract(startOfCentury).Days;

            // Calculate the number of milliseconds after midnight, divided by 3.3333...
            Int32 theMilliSeconds = (int)(theNow.Subtract(DateTime.Today).TotalMilliseconds / 3.333333333333333333);

            // Make a new GUID and save it as a byte array.
            Byte[] aBuffer = Guid.NewGuid().ToByteArray();
            Byte[] theDaysAsBytes = BitConverter.GetBytes(theDays);

            // Exchange byte 10 and 11 for the current days from 1900-01-01.
            aBuffer[10] = theDaysAsBytes[1];
            aBuffer[11] = theDaysAsBytes[0];

            // Exchange byte 12-15 for the no of milliseconds after midnight. (Divided by 3.3333...)
            Byte[] theMilliSecondsAsBytes = BitConverter.GetBytes(theMilliSeconds);
            aBuffer[12] = theMilliSecondsAsBytes[3];
            aBuffer[13] = theMilliSecondsAsBytes[2];
            aBuffer[14] = theMilliSecondsAsBytes[1];
            aBuffer[15] = theMilliSecondsAsBytes[0];

            Guid g = new Guid(aBuffer);
            return g;
        }
    }
}

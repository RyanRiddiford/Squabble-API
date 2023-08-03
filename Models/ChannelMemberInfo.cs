using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Squabble.Models
{
    public class ChannelMemberInfo
    {

        public string UserName
        {
            get; set;
        }

        public string FirstName
        {
            get; set;
        }

        public string MiddleName
        {
            get; set;
        }
        public string Surname
        {
            get; set;
        }
        public string ACSId { get; set; }


        public string Role 
        { 
        get;set;
        }
        
    }

}

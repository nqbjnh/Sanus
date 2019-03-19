using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanus.Model
{
    public class User : RealmObject
    {
        [PrimaryKey]
        public string Email { get; set; }
        public string Name { get; set; }
    }
}

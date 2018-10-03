using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Domain.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // TODO add many-many connection with users
    }
}

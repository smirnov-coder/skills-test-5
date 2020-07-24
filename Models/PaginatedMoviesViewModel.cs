using SkillsTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Models
{
    public class PaginatedMoviesViewModel
    {
        public PagingInfo Pagination { get; set; }

        public IList<Movie> Movies { get; set; }
    }
}

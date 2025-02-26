using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using BulkyBook.DataAccess.Data;
using System.Runtime.Intrinsics.Arm;

namespace BulkyBook.DataAccess.Repository
{
	public class CategoryRepository : Repository<Category>,ICategoryRepository
	{
		private ApplicationDbContext _db;
		public CategoryRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Category category)
		{
			_db.Categories.Update(category);
		}
	}
}

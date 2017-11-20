using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFManagement.Repository;
using EFManagement.Domain;

namespace ContextManagement.Web.Controllers
{ 
    public class ProductController : Controller
    {
        //private NorthwindEntities db = new NorthwindEntities();

        private IRepository<Product> productRepository = new Repository<Product>();
        private IRepository<Supplier> supplierRepository = new Repository<Supplier>();
        private IRepository<Category> categoryRepository = new Repository<Category>();
        //
        // GET: /Product/

        public ViewResult Index()
        {
            var products = productRepository.Fetch().Include("Category").Include("Supplier");
            return View(products.ToList());
        }

        //
        // GET: /Product/Details/5

        public ViewResult Details(int id)
        {
            Product product = productRepository.Single(p => p.ProductID == id);
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(categoryRepository.Fetch(), "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(supplierRepository.Fetch(), "SupplierID", "CompanyName");
            return View();
        } 

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Add(product);
                productRepository.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CategoryID = new SelectList(categoryRepository.Fetch(), "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(supplierRepository.Fetch(), "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }
        
        //
        // GET: /Product/Edit/5
 
        public ActionResult Edit(int id)
        {
            Product product = productRepository.Single(p => p.ProductID == id);
            ViewBag.CategoryID = new SelectList(categoryRepository.Fetch(), "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(supplierRepository.Fetch(), "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Attach(product);
                //db.ObjectStateManager.ChangeObjectState(product, EntityState.Modified);
                productRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(categoryRepository.Fetch(), "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(supplierRepository.Fetch(), "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        //
        // GET: /Product/Delete/5
 
        public ActionResult Delete(int id)
        {
            Product product = productRepository.Single(p => p.ProductID == id);
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = productRepository.Single(p => p.ProductID == id);
            productRepository.Delete(product);
            productRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
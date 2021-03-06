﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Filters;
using StandardWebApi.Models;
using StandardWebApi.Security;

namespace StandardWebApi.Controllers
{

    public class ProductsController : ApiController
    {
        private StandardWebApiContext db = new StandardWebApiContext();
        HttpRequestBase _request;


        // GET: api/Products/GetProducts
        public IQueryable<ProductDTO> GetProducts()
        {
            var products = from p in db.Products
                           select new ProductDTO()
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Category = p.Category,
                               Price = p.Price
                           };
            return products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }


        [HttpGet]
        // GET: api/Products/GenerateToken
        [ResponseType(typeof(string))]
        public IHttpActionResult GenerateToken()
        {
            var generatedToken = TokenGenerator.GetToken(12);
            return Ok(generatedToken);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != product.Id)
                return BadRequest();

            db.Entry(product).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Products.Add(product);
            await db.SaveChangesAsync();

            var dto = new ProductDTO()
            {
                Id = product.Id,
                Category = product.Category,
                Name = product.Name,
                Price = product.Price
            };

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, dto);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MiMovil.Models;
using MiMovil.Services;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiMovil.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbcontext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbcontext context,IWebHostEnvironment environment )
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {  return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null) {
                ModelState.AddModelError("foto", " Es requerida");

            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            //Guardar la imagen 
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using(var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);

            }

            //guarda en la base
            Product product = new Product()
            {
                Nombre = productDto.Nombre,
                Marca = productDto.Marca,
                Categoria = productDto.Categoria,
                Precio = productDto.Precio,
                Descripcion = productDto.Descripcion,
                ImageFileName =  newFileName,
                CreatedAt = DateTime.Now
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index","Products");

        }
        //Editar 
        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");

            }

            var productoDto = new ProductDto()
            {
                Nombre = product.Nombre,
                Marca = product.Marca,
                Categoria = product.Categoria,
                Precio = product.Precio,
                Descripcion = product.Descripcion


            };

            ViewData["ProductId"] = product.Id;

            ViewData["ImageFileName"] = product.ImageFileName;

            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
            return View(productoDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;

                ViewData["ImageFileName"] = product.ImageFileName;

                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
                return View(productDto);
            }

            //Actualizar imagen 
            string newFileName = product.ImageFileName;
            if(productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);

                }

                //Borrar la imagen vieja 
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);


            }

            // Actualizar El producto en la base de datos 
                product.Nombre = productDto.Nombre;
                product.Marca = productDto.Marca;
                product.Categoria = productDto.Categoria;
                product.Precio = productDto.Precio;
                product.Descripcion = productDto.Descripcion;
                product.ImageFileName = newFileName;

            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }
        //Eliminar
        public IActionResult Delete (int id)
        {
            var product = context.Products.Find(id);
                if (product == null)
            {
               return  RedirectToAction("Index", "Products");
            }
            string imageFullPath = environment.WebRootPath + "/products/" +product.ImageFileName;
            System.IO.File.Delete(imageFullPath);
            context.Products.Remove(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

    }
}

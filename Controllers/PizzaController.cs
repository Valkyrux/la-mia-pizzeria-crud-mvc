using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_model.Models;
using System.Net;

namespace la_mia_pizzeria_model.Controllers
{
    public class PizzaController : Controller
    {
        static Pizzeria pizzeria = new Pizzeria("Da Luigi");

        public IActionResult Index()
        {
            ViewData["nomePizzeria"] = pizzeria.Nome;
            return View(pizzeria.listaPizze);
        }

        public IActionResult ShowPizza(int id)
        {
            Pizza? pizzaCercata = pizzeria.listaPizze.Find(item => item.Id == id);
            if(pizzaCercata == null)
            {
                ViewData["Titolo"] = "Pizza Not Found!";
                return View("PizzaNotFound");
            }
            else
            {
                ViewData["nomePizzeria"] = pizzeria.Nome;
                return View(pizzaCercata);
            }
        }

        public IActionResult CreatePizza()
        {
            Pizza nuovaPizza = new Pizza();
            return View(nuovaPizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidatePizza(Pizza nuovaPizza)
        {
            if (!ModelState.IsValid)
            {
                return View("CreatePizza", nuovaPizza);
            }

            if(nuovaPizza.formFile != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileInfo newFileInfo = new FileInfo(nuovaPizza.formFile.FileName);
                string fileName = String.Format("{0}{1}", nuovaPizza.Name, newFileInfo.Extension);
                string FullPathName = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(FullPathName, FileMode.Create))
                {
                    nuovaPizza.formFile.CopyTo(stream);
                    stream.Close();
                }
                nuovaPizza.ImgPath = String.Format("{0}", fileName);
            }
            pizzeria.addPizza(nuovaPizza);
            return View("ShowPizza", nuovaPizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidateUpdatePizza(Pizza nuovaPizza)
        {
            if (!ModelState.IsValid)
            {
                return View("CreatePizza", nuovaPizza);
            }

            if (nuovaPizza.formFile != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileInfo newFileInfo = new FileInfo(nuovaPizza.formFile.FileName);
                string fileName = String.Format("{0}{1}", nuovaPizza.Name, newFileInfo.Extension);
                string FullPathName = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(FullPathName, FileMode.Create))
                {
                    nuovaPizza.formFile.CopyTo(stream);
                    stream.Close();
                }
                nuovaPizza.ImgPath = String.Format("{0}", fileName);
            }
            pizzeria.delPizza(nuovaPizza.Id);
            pizzeria.addPizza(nuovaPizza);
            return View("ShowPizza", nuovaPizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePizza(int? id)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:5084/Pizza/DelPizza");
            var postData = "id=" + Uri.EscapeDataString(id.ToString());
            var data = System.Text.Encoding.UTF8.GetBytes(postData);

            request.Method = "DELETE";

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
;
            if(new StreamReader(response.GetResponseStream()).ReadToEnd() == "true")
            {
                return RedirectToAction("Index", pizzeria.listaPizze);
            }
            return View("PizzaNotFound");
        }

        [HttpDelete]
        public bool DelPizza(int? id)
        {
            pizzeria.delPizza(id);
            return true;
        }

        public IActionResult UpdatePizza(int? id)
        {
            if(id == null)
            {
                return View("PizzaNotFound");
            }
            Pizza pizzaDaModificare = (pizzeria.listaPizze.Where(item => item.Id == id).ToArray())[0];
            return View("EditPizza", pizzaDaModificare);
        }
    }
}

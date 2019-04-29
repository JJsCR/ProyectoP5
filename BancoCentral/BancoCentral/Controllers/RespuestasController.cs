using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BancoCentral.Models;

namespace BancoCentral.Controllers
{
    public class RespuestasController : Controller
    {
        private BancoCentralEntities db = new BancoCentralEntities();

        // GET: Respuestas
        public async Task<ActionResult> Index()
        {
            var respuesta = db.Respuesta.Include(r => r.Consulta);
            return View(await respuesta.ToListAsync());
        }

        // GET: Respuestas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta respuesta = await db.Respuesta.FindAsync(id);
            if (respuesta == null)
            {
                return HttpNotFound();
            }
            return View(respuesta);
        }

        // GET: Respuestas/Create
        public ActionResult Create()
        {
            ViewBag.consultaId = new SelectList(db.Consulta, "idConsulta", "nombre");
            return View();
        }

        // POST: Respuestas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "idRespuesta,nombre,respuesta1,consultaId")] Respuesta respuesta)
        {
            if (ModelState.IsValid)
            {
                db.Respuesta.Add(respuesta);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.consultaId = new SelectList(db.Consulta, "idConsulta", "nombre", respuesta.consultaId);
            return View(respuesta);
        }

        // GET: Respuestas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta respuesta = await db.Respuesta.FindAsync(id);
            if (respuesta == null)
            {
                return HttpNotFound();
            }
            ViewBag.consultaId = new SelectList(db.Consulta, "idConsulta", "nombre", respuesta.consultaId);
            return View(respuesta);
        }

        // POST: Respuestas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "idRespuesta,nombre,respuesta1,consultaId")] Respuesta respuesta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(respuesta).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.consultaId = new SelectList(db.Consulta, "idConsulta", "nombre", respuesta.consultaId);
            return View(respuesta);
        }

        // GET: Respuestas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta respuesta = await db.Respuesta.FindAsync(id);
            if (respuesta == null)
            {
                return HttpNotFound();
            }
            return View(respuesta);
        }

        // POST: Respuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Respuesta respuesta = await db.Respuesta.FindAsync(id);
            db.Respuesta.Remove(respuesta);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

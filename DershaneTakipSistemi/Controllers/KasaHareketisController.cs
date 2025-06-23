using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;

namespace DershaneTakipSistemi.Controllers
{
    public class KasaHareketisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KasaHareketisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KasaHareketis
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.KasaHareketleri.Include(k => k.Ogrenci).Include(k => k.Personel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: KasaHareketis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kasaHareketi = await _context.KasaHareketleri
                .Include(k => k.Ogrenci)
                .Include(k => k.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kasaHareketi == null)
            {
                return NotFound();
            }

            return View(kasaHareketi);
        }

        // GET: KasaHareketis/Create
        public IActionResult Create()
        {
            ViewData["OgrenciId"] = new SelectList(_context.Ogrenciler, "Id", "Ad");
            ViewData["PersonelId"] = new SelectList(_context.Personeller, "Id", "Ad");
            return View();
        }

        // POST: KasaHareketis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Aciklama,Tarih,Tutar,HareketYonu,Kategori,OdemeYontemi,OgrenciId,PersonelId")] KasaHareketi kasaHareketi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kasaHareketi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OgrenciId"] = new SelectList(_context.Ogrenciler, "Id", "Ad", kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = new SelectList(_context.Personeller, "Id", "Ad", kasaHareketi.PersonelId);
            return View(kasaHareketi);
        }

        // GET: KasaHareketis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kasaHareketi = await _context.KasaHareketleri.FindAsync(id);
            if (kasaHareketi == null)
            {
                return NotFound();
            }
            ViewData["OgrenciId"] = new SelectList(_context.Ogrenciler, "Id", "Ad", kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = new SelectList(_context.Personeller, "Id", "Ad", kasaHareketi.PersonelId);
            return View(kasaHareketi);
        }

        // POST: KasaHareketis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Aciklama,Tarih,Tutar,HareketYonu,Kategori,OdemeYontemi,OgrenciId,PersonelId")] KasaHareketi kasaHareketi)
        {
            if (id != kasaHareketi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kasaHareketi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KasaHareketiExists(kasaHareketi.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OgrenciId"] = new SelectList(_context.Ogrenciler, "Id", "Ad", kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = new SelectList(_context.Personeller, "Id", "Ad", kasaHareketi.PersonelId);
            return View(kasaHareketi);
        }

        // GET: KasaHareketis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kasaHareketi = await _context.KasaHareketleri
                .Include(k => k.Ogrenci)
                .Include(k => k.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kasaHareketi == null)
            {
                return NotFound();
            }

            return View(kasaHareketi);
        }

        // POST: KasaHareketis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kasaHareketi = await _context.KasaHareketleri.FindAsync(id);
            if (kasaHareketi != null)
            {
                _context.KasaHareketleri.Remove(kasaHareketi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KasaHareketiExists(int id)
        {
            return _context.KasaHareketleri.Any(e => e.Id == id);
        }
    }
}

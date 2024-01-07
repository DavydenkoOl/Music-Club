using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music_Club.Models;
using Music_Club.Repository;

namespace Music_Club.Controllers
{
    public class MusicClipsController : Controller
    {
        IRepository<MusicClip> _context;
        IWebHostEnvironment _appEnvironment;
        public MusicClipsController(IRepository<MusicClip> context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: MusicClips
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetList());
        }

        // GET: MusicClips/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var musicClip = await _context.Clips
        //    //    .FirstOrDefaultAsync(m => m.Id == id);
        //    if (musicClip == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(musicClip);
        //}

        // GET: MusicClips/Create
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> SelectedVideo(int id)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Append("Selected_video", id.ToString(), option);

            return RedirectToAction("Index",await _context.GetList());
        }
        // POST: MusicClips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ReleaseDate,Artist,Genre,Id_user")] MusicClip musicClip, IFormFile? uploadedFile)
        {
            if (uploadedFile != null)
            {
                // Путь к папке Files
                string path = "/video/" + uploadedFile.FileName; // имя файла

                // Сохраняем файл в папку images в каталоге wwwroot
                // Для получения полного пути к каталогу wwwroot
                // применяется свойство WebRootPath объекта IWebHostEnvironment
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                }




                musicClip.Path_Video = uploadedFile.FileName;
            }
            if (ModelState.IsValid)
            {
                _context.Create(musicClip);
                await _context.Save();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Create", "MusicClips", musicClip);
        }

        // GET: MusicClips/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var musicClip = await _context.Clips.FindAsync(id);
        //    if (musicClip == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(musicClip);
        //}

        // POST: MusicClips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ReleaseDate,Artist,Genre,Id_user")] MusicClip musicClip)
        //{
        //    if (id != musicClip.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(musicClip);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MusicClipExists(musicClip.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(musicClip);
        //}

        // GET: MusicClips/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var musicClip = await _context.Clips
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (musicClip == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(musicClip);
        //}

        // POST: MusicClips/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var musicClip = await _context.Clips.FindAsync(id);
        //    if (musicClip != null)
        //    {
        //        _context.Clips.Remove(musicClip);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MusicClipExists(int id)
        //{
        //    return _context.Clips.Any(e => e.Id == id);
        //}
    }
}

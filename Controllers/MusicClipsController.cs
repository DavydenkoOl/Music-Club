using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Music_Club.Models;
using Music_Club.Repository;

namespace Music_Club.Controllers
{
    public class MusicClipsController : Controller
    {
        IRepository<MusicClip> _context;
        IWebHostEnvironment _appEnvironment;
        IRepository<Genre> _context_genre;
        public MusicClipsController(IRepository<MusicClip> context, IWebHostEnvironment appEnvironment, IRepository<Genre> context_genre)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _context_genre = context_genre;
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
        public async Task< IActionResult> Create()
        {
            MusicClipView model = new MusicClipView();
            model.GenreList = await _context_genre.GetList();
            return View(model);
        }
        public async Task<IActionResult> SelectedVideo(int id)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            var tmp=await _context.GetList();
            int selected_index=0;
            for (int i = 0; i < tmp.Count; i++)
            { 
                if (tmp[i].Id == id)
                    selected_index = i; 
            }
            int previous_video, next_video=0;
            if (selected_index != 0)
            {
                previous_video = tmp[selected_index - 1].Id;
            }
            else
            {
                previous_video = tmp[tmp.Count - 1].Id;
            }
            if(selected_index == tmp.Count - 1 )
            {
                next_video = tmp[0].Id;
            }
            else if(selected_index == 0 &&  tmp.Count == 1) { next_video = id; }
            else if(selected_index >= 0 && selected_index < tmp.Count - 1) { next_video = tmp[selected_index + 1].Id; }
            Response.Cookies.Append("Selected_video", id.ToString(), option);
            Response.Cookies.Append("previous_video", previous_video.ToString(), option);
            Response.Cookies.Append("next_video", next_video.ToString(), option);
            return RedirectToAction("Index",await _context.GetList());
        }
        // POST: MusicClips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(1000000000)]
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
                musicClip.Genre = musicClip.Genre.Trim();
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

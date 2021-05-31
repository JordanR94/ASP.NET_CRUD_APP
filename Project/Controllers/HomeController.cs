using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ChinookMVC.Models;
using JR.Shared;
using System.Configuration;
using System.Data.SqlClient;

namespace ChinookMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        Chinook db = new Chinook();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Album> albums = db.Albums.ToList();

            var model = albums;

            return View(model);
        }

        public IActionResult Tracks(int id)
        {
            List<Track> tracks = db.Tracks.ToList();

            var model = from t in tracks
                        where t.AlbumID == id
                        select new HomeIndexViewModel
                        {
                            track = t
                        };

            return View(model);
        }

        public IActionResult UpdateTrack(int? id)
        {
            List<Track> tracks = db.Tracks.ToList();

            var model = from t in tracks
                        where t.TrackID == id
                        select new HomeIndexViewModel
                        {
                            track = t
                        };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTrack(int id)
        {
            var trackModel = await db.Tracks.FirstOrDefaultAsync(t => t.TrackID == id);

            if (await TryUpdateModelAsync<Track>(trackModel, "", t => t.Name))
            {
                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(trackModel);
        }


        public IActionResult AlbumDetail(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a product ID in the route");
            }
            List<Album> albums = db.Albums.ToList();
            List<Artist> artists = db.Artists.ToList();
            List<Track> tracks = db.Tracks.ToList();

            var model = from alb in albums
                        join art in artists on alb.ArtistID equals art.ArtistID
                        join t in tracks on alb.AlbumID equals t.AlbumID
                        where alb.AlbumID == id
                        select new HomeIndexViewModel
                        {
                            album = alb,
                            artist = art,
                            track = t
                        };

            if (model == null)
            {
                return NotFound($"Album with ID of {id} not found.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateAlbum(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass an ID in the route");
            }
            List<Album> albums = db.Albums.ToList();
            List<Artist> artists = db.Artists.ToList();
            List<Track> tracks = db.Tracks.ToList();

            var model = from alb in albums
                        join art in artists on alb.ArtistID equals art.ArtistID
                        join t in tracks on alb.AlbumID equals t.AlbumID
                        where alb.AlbumID == id
                        select new HomeIndexViewModel
                        {
                            album = alb,
                            artist = art,
                            track = t
                        };

            if (model == null)
            {
                return NotFound($"Album with ID of {id} not found.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAlbum(int AlbID)
        {

            var albumModel = await db.Albums.FirstOrDefaultAsync(a => a.AlbumID == AlbID);
            var artistModel = await db.Artists.FirstOrDefaultAsync(a => a.ArtistID == albumModel.ArtistID);

            if (await TryUpdateModelAsync<Album>(albumModel, "", a => a.Title)
            && await TryUpdateModelAsync<Artist>(artistModel, "", a => a.Name))
            {
                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(albumModel);
        }
        [HttpGet]
        public IActionResult CreateTrack()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrack([Bind("Name")] Track track)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Add(track);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(track);
        }
        [HttpGet]
        public IActionResult CreateArtist()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArtist([Bind("Name")] Artist artist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Add(artist);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(artist);
        }
        [HttpGet]
        public IActionResult CreateAlbum()
        {

            var model = new HomeIndexViewModel
            {
                artists = db.Artists.ToList()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAlbum([Bind("Title, ArtistID")] Album album)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Add(album);
                    await db.SaveChangesAsync();
                    return RedirectToAction("AddTracksToAlb", album);
                }
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(album);
        }
        [HttpGet]
        public IActionResult AddAnotherTrack(int id)
        {

            var model = new HomeIndexViewModel
            {
                tracks = db.Tracks.ToList(),
                AlbumId = id
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnotherTrack(int? trackID)
        {
            var trackModel = await db.Tracks.FirstOrDefaultAsync(t => t.TrackID == trackID);

            if (await TryUpdateModelAsync<Track>(trackModel, "", t => t.AlbumID))
            {
                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View();

        }

        [HttpGet]
        public IActionResult AddTracksToAlb(int? AlbID)
        {
            var model = new HomeIndexViewModel
            {
                tracks = db.Tracks.ToList(),
                albums = db.Albums.OrderByDescending(a => a.AlbumID).ToList()
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTracksToAlb(int trackID)
        {
            var trackModel = await db.Tracks.FirstOrDefaultAsync(t => t.TrackID == trackID);

            if (await TryUpdateModelAsync<Track>(trackModel, "", t => t.AlbumID))
            {
                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View();

        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            List<Album> albums = db.Albums.ToList();
            var model = from alb in albums
                        where alb.AlbumID == id
                        select new HomeIndexViewModel
                        {
                            album = alb
                        };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var alb = await db.Albums.FindAsync(id);
            db.Albums.Remove(alb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

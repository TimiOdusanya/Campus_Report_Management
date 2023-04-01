using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportManagement.Data;
using ReportManagement.Models;
using ReportManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReportManagement.Controllers
{
    [Authorize]
    public class ComplaintController : Controller
    {
        
            private readonly ApplicationDbContext _db;
            private readonly IWebHostEnvironment _webHostEnvironment;

            public ComplaintController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
            {

                _db = db;
                _webHostEnvironment = webHostEnvironment;
            }
            public IActionResult Index()
            {
                IEnumerable<Complaint> objList = _db.Complaint.Include(u => u.Category);

                //foreach(var obj in objList)
                //{
                //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
                //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
                //};
                return View(objList);
            }

            //GET -UPSERT

            public IActionResult Upsert(int? id)
            {
                //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
                //{
                //    Text = i.Name,
                //    Value = i.Id.ToString()

                //} );

                //ViewBag.CategoryDropDown = CategoryDropDown;

                // Complaint complaint = new Complaint();

                ComplaintVM complaintVM = new ComplaintVM()
                {
                    Complaint = new Complaint(),
                    CategorySelectList = _db.Category.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),

                };

                if (id == null)
                {
                    //this is for create
                    return View(complaintVM);
                }
                else
                {
                    complaintVM.Complaint = _db.Complaint.Find(id);
                    if (complaintVM.Complaint == null)
                    {
                        return NotFound();
                    }
                    return View(complaintVM);
                }

            }

            //POST - UPSERT
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Upsert(ComplaintVM complaintVM)

            {
                if (ModelState.IsValid)
                {
                    var files = HttpContext.Request.Form.Files;
                    string webRootPath = _webHostEnvironment.WebRootPath;

                    if (complaintVM.Complaint.Id == 0)
                    {
                        //Creating
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        complaintVM.Complaint.Image = fileName + extension;

                        _db.Complaint.Add(complaintVM.Complaint);

                    }
                    else
                    {
                        //updating
                        var objFromDb = _db.Complaint.AsNoTracking().FirstOrDefault(u => u.Id == complaintVM.Complaint.Id);

                        if (files.Count > 0)
                        {
                            string upload = webRootPath + WC.ImagePath;
                            string fileName = Guid.NewGuid().ToString();
                            string extension = Path.GetExtension(files[0].FileName);

                            var oldFile = Path.Combine(upload, objFromDb.Image);

                            if (System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }

                            using (var filestream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                            {
                                files[0].CopyTo(filestream);
                            }

                            complaintVM.Complaint.Image = fileName + extension;
                        }
                        else
                        {
                            complaintVM.Complaint.Image = objFromDb.Image;
                        }
                        _db.Complaint.Update(complaintVM.Complaint);

                    }

                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
                complaintVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });   

                return View(complaintVM);
            }

            //GET - DELETE
            public IActionResult Delete(int? id)
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                Complaint complaint = _db.Complaint.Include(u => u.Category).FirstOrDefault(u => u.Id == id);
                if (complaint == null)
                {
                    return NotFound();

                }

                return View(complaint);
            }
            //POST - DELETE
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeletePost(int? id)
            {
                var obj = _db.Complaint.Find(id);
                if (obj == null)
                {
                    return NotFound();
                }
                string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
                var oldFile = Path.Combine(upload, obj.Image);

                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                _db.Complaint.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

     
    }
}

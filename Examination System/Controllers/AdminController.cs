﻿using Examination_System.Models;
using Examination_System.Repository;
using Examination_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination_System.Controllers
{
    [Authorize(Roles = "Admins")]
    public class AdminController : Controller
    {
        IAdminRepo adminRepo;
        public AdminController(IAdminRepo _adminRepo)
        {
            adminRepo = _adminRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowAdmins()
        {
            var model = adminRepo.showAdmins();
            return View(model);
        }
        public IActionResult createAdmins()
        {
            return View();
        }
        [HttpPost]
        public IActionResult createAdmins(User user)
        {
            if (ModelState.IsValid)
            {
                adminRepo.AddUser(user);

                adminRepo.AddRole(user);
                 Admin admin = new Admin();
                {
                    admin.UserId = user.Id;
                }

                adminRepo.AddAdmin(admin);
                return RedirectToAction("ShowAdmins");
            }
            return View(user);
        }

        public IActionResult EditAdmin(int? id)
        {

            var model = adminRepo.getUser(id.Value);
              return View(model);

        }


        [HttpPost]
        public IActionResult EditAdmin(User u)
        {
            adminRepo.EditUser(u);
            return RedirectToAction("ShowAdmins");

        }

        public IActionResult DeleteAdmin(int id)
        {

            var model = adminRepo.getUser(id);
            adminRepo.DeleteUser(model);
            return RedirectToAction("ShowAdmins");
        }

        public IActionResult ShowInstructor()
        {
            var model = adminRepo.GetUsers();
            return View(model);
        }
        public IActionResult createInstructor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult createInstructor(User user)
        {
            if (ModelState.IsValid)
            {

                adminRepo.AddUser(user);

                adminRepo.AddRoleInstructor(user);

                adminRepo.AddInstructor(user);
                return RedirectToAction("ShowInstructor");
            }
            return View(user);
        }

        public IActionResult EditInstructor(int? id)
        {
           
            var model = adminRepo.getUser(id.Value);
         
            return View(model);

        }


        [HttpPost]
        public IActionResult EditInstructor(User user)
        {
            adminRepo.EditInstructor(user);
            return RedirectToAction("ShowInstructor");

        }
        public IActionResult DeleteInstructor(int id)
        {
            if (id == null)
                return BadRequest();
            var model = adminRepo.getUser(id);
            if (model == null)
                return NotFound();

            adminRepo.DeleteUser(model);
            return RedirectToAction("ShowInstructor");
        }

        public IActionResult ShowStudent()
        {
            var model = adminRepo.getStudents();
            return View(model);
        }
        public IActionResult createStudent()
        {
            ViewBag.deptList = adminRepo.getDepartments();
            ViewBag.deptList2 = adminRepo.getBranches();

            return View();
        }


        [HttpPost]
        public IActionResult createStudent(RegisterStdVM u)
        {
            if (ModelState.IsValid)
            {

               User user =  adminRepo.AddStudent(u);
                adminRepo.AddRoleStudent(user);

                adminRepo.AddStudent(user, u);
                return RedirectToAction("ShowStudent");
            }
            return View(u);
        }

        public IActionResult EditStudent(int? id)
        {
            ViewBag.deptList = adminRepo.getDepartments();
            ViewBag.deptList2 = adminRepo.getBranches();

            var model = adminRepo.getUserStudent(id.Value);
            return View(model);

        }


        [HttpPost]
        public IActionResult EditStudent(User u)
        {
            var old = adminRepo.getOldStudentUsers(u);
            adminRepo.EditUserStudent(u,old);
            return RedirectToAction("ShowStudent");

        }

        public IActionResult DeleteStudent(int id)
        {
            var model = adminRepo.getUser(id);

            adminRepo.DeleteUserStudent(model);
           
            return RedirectToAction("ShowStudent");
        }

        public IActionResult ShowBranches()
        {
            var model=adminRepo.ShowBranches();
            return View(model);
        }

        public IActionResult createBranches()
        {
            //ViewBag.deptList = db.Users.Where(a => a.Id == a.Instructor.UserId).Include(a => a.Instructor).ToList();
            ViewBag.deptList = adminRepo.getusersbranches();
            return View();
        }
        [HttpPost]
        public IActionResult createBranches(Branch model)
        {
            if (model.BranchName?.Length > 1)
            {
                adminRepo.AddBranch(model);
                return RedirectToAction("ShowBranches");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult EditBranch(int? id)
        {
            ViewBag.deptList = adminRepo.getusersbranches();

            if (id == null)
                return BadRequest();
            var model = adminRepo.getBranchById(id.Value);
            if (model == null)
                return NotFound();
            return View(model);

        }
        [HttpPost]
        public IActionResult EditBranch(Branch u)
        {
            adminRepo.UpdateBranch(u);
            return RedirectToAction("ShowBranches");

        }

        public IActionResult DeleteBranch(int id)
        {
            if (id == null)
                return BadRequest();
            var model = adminRepo.getBranchById(id);
            if (model == null)
                return NotFound();

          
            adminRepo.DeleteBranch(model);
            return RedirectToAction("ShowBranches");
        }

        public IActionResult ShowDepartment()
        {
            var model = adminRepo.getDepartmentList();
            return View(model);
        }

        public IActionResult createDepartment()
        {

            ViewBag.deptList = adminRepo.getusersbranches();
            return View();
        }
        [HttpPost]
        public IActionResult createDepartment(Department model)
        {
            if (model.DepartmentName?.Length > 1)
            {
                adminRepo.AddDepartment(model);
                return RedirectToAction("ShowDepartment");
            }
            else
            {
                return View(model);
            }
        }


        public IActionResult EditDepartment(int? id)
        {
            ViewBag.deptList = adminRepo.getusersbranches();
            if (id == null)
                return BadRequest();
            var model = adminRepo.GetDepartmentById(id.Value);
            if (model == null)
                return NotFound();
            return View(model);

        }
        [HttpPost]
        public IActionResult EditDepartment(Department u)
        {
          adminRepo.UpdateDepartment(u);
            return RedirectToAction("ShowDepartment");

        }

        public IActionResult DeleteDepartment(int id)
        {
            if (id == null)
                return BadRequest();
            var model = adminRepo.GetDepartmentById(id);
            if (model == null)
                return NotFound();

            adminRepo.removeDepartment(model);
            return RedirectToAction("ShowDepartment");
        }

        public IActionResult ShowCourses()
        {
            var model = adminRepo.GetListOfCourses();
            return View(model);

        }



    }


}

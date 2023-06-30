using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/
            IUnitOfWork unitOfWork)
        {
           // _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async  Task<IActionResult> Index()
        {
            //ViewData["Message"] = "ViewData from Department controller!";
            //ViewBag.Message2 = "ViewBag from Department controller!";
            //TempData.Keep("Message");
            var Department =await _unitOfWork.DepartmentRepository.GetAll();
            return View(Department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "TempData in Create to Index from Department controller!";
                await _unitOfWork.DepartmentRepository.Add(department);
                return RedirectToAction("Index");
            }
            return View(department);
        }
        public async  Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();

            var department =await _unitOfWork.DepartmentRepository.Get(id);

            if (department == null)
                return NotFound();

            return View(department);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return NotFound();

            var department =await _unitOfWork.DepartmentRepository.Get(id);

            if (department == null)
                return NotFound();

            return View(department);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id , Department department)
        {
            if(id != department.Id)
                return NotFound();
            if(ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.DepartmentRepository.Update(department);
                    return RedirectToAction("Index");
                }catch(Exception ex)
                {
                    return View(department);
                }
            }
            return View(department);

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var department =await _unitOfWork.DepartmentRepository.Get(id);

            if (department == null)
                return NotFound();

             await _unitOfWork.DepartmentRepository.Delete(department);
            return RedirectToAction("Index");
        }
    }
}

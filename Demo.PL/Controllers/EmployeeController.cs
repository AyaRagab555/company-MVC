using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Demo.BLL.Repositories;
using Demo.PL.Models;
using AutoMapper;
using System.Collections.Generic;
using Demo.PL.Helper;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public EmployeeController(/*IEmployeeRepository employeeRepository , IDepartmentRepository departmentRepository*/
            IUnitOfWork unitOfWork
            , IMapper  mapper)
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue = "")
        {
            IEnumerable<Employee> employees;
            if (String.IsNullOrEmpty(SearchValue))
            {
                 employees = await _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.Search(SearchValue);
            }
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();
            //var mappedEmployees = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments =await _unitOfWork.DepartmentRepository.GetAll();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                //var mappedEmployee = new Employee()
                //{
                //    Id = employee.Id,
                //    Name = employee.Name,
                //    Salary = employee.Salary,
                //    HireDate = employee.HireDate,
                //    DepartmentId = employee.DepartmentId,
                //    Email = employee.Email,
                //    Address = employee.Address,
                //    Age = employee.Age,
                //    IsActive = employee.IsActive,
                //    PhoneNumber = employee.PhoneNumber,
                //};
                employeeViewModel.ImageUrl = DocumentSetting.UploadFile(employeeViewModel.Image, "imgs");
                var mappedEmployee = _mapper.Map<Employee>(employeeViewModel);
                await _unitOfWork.EmployeeRepository.Add(mappedEmployee);
                return RedirectToAction("Index");
            }
            return View(employeeViewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();

            var employee = await _unitOfWork.EmployeeRepository.Get(id);


            var departmentName = await _unitOfWork.EmployeeRepository.GetDepartmentNameByEmpId(id);
            var mappedEmployee = _mapper.Map<EmployeeViewModel>(employee);

            if (employee == null)
                return NotFound();

            return View(mappedEmployee);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return NotFound();

            var employee = await _unitOfWork.EmployeeRepository.Get(id);
            var mappedEmployee = _mapper.Map<EmployeeViewModel>(employee);

            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();

            if (employee == null)
                return NotFound();

            return View(mappedEmployee);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    employeeViewModel.ImageUrl = DocumentSetting.UploadFile(employeeViewModel.Image, "imgs");
                    var mappedEmployee = _mapper.Map<Employee>(employeeViewModel);
                    await _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(employeeViewModel);
                }
            }
            return View(employeeViewModel);

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var employee = await _unitOfWork.EmployeeRepository.Get(id);

            if (employee == null)
                return NotFound();
            
            DocumentSetting.Delete( "imgs" , employee.ImageUrl);

            await _unitOfWork.EmployeeRepository.Delete(employee);
            return RedirectToAction("Index");
        }
    }
}

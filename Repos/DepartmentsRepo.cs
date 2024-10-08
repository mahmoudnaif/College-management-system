﻿using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{
    public class DepartmentsRepo : IDepartmentsRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public DepartmentsRepo(CollegeDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<DepartmentDTO>> AddDepartment(string departmnetName)
        {
            if(departmnetName.Trim().Length == 0)
                return new CustomResponse<DepartmentDTO>(400,"Department name must be specefied");

            Department departmentExists = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentName == departmnetName);

            if(departmentExists != null)
                return  new CustomResponse<DepartmentDTO>(409, "Department already exists");

            Department department = new Department() {DepartmentName = departmnetName};

            try
            {
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
                DepartmentDTO departmentDTO =  _mapper.Map<DepartmentDTO>(department);
                return new CustomResponse<DepartmentDTO>(201, "Deparment added successfully", departmentDTO);
            }
            catch
            {
                return new CustomResponse<DepartmentDTO>(500, "Internal server error");
            }


        }

        public async Task<CustomResponse<bool>> DeleteDepartment(int departmentId)
        {
            Department department = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentId == departmentId);

            if (department == null)
                return new CustomResponse<bool>(409, "Department does not exist");


            try
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Department removed successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<DepartmentDTO>> EditDepartmentName(EditDepartmentNameModel editModel )
        {
            if (editModel.newName.Trim().Length == 0)
                return new CustomResponse<DepartmentDTO>(400, "New name must be specefied");

            Department departmentExists = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentName == editModel.newName);

            if (departmentExists != null)
                return new CustomResponse<DepartmentDTO>(409,"Name already exists");

            Department department = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentId == editModel.departmentId);

            if (department == null)
                return new CustomResponse<DepartmentDTO>(404, "Department doesn't exist");


            department.DepartmentName = editModel.newName;

            try
            {
                await _context.SaveChangesAsync();
                DepartmentDTO departmentDTO = _mapper.Map<DepartmentDTO>(department);
                return new CustomResponse<DepartmentDTO>(200, "Department name edited successfully", departmentDTO);
            }
            catch
            {
                return new CustomResponse<DepartmentDTO>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<List<DepartmentDTO>>> GetDepartments()
        {
            List<Department> departments = await _context.Departments.ToListAsync();


            if(departments == null || !departments.Any())
                return new CustomResponse<List<DepartmentDTO>>(404,"No departments found");


            List<DepartmentDTO> departmentsDTO = _mapper.Map<List<DepartmentDTO>>(departments);

            return new CustomResponse<List<DepartmentDTO>>(200, "Departments retreived", departmentsDTO);

        }
    }
}

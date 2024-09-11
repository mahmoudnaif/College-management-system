using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Mapper;
using College_managemnt_system.models;
using College_managemnt_system.Repos.Utilities;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using System.Globalization;

namespace College_managemnt_system.Repos
{
    public class CSVParser : ICSVParser
    {
        private readonly CollegeDBContext _context;
        private readonly UtilitiesRepo _utilitiesRepo;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public CSVParser(CollegeDBContext context,UtilitiesRepo utilitiesRepo,IMapper mapper, IPasswordService passwordService)
        {
            _context = context;
            _utilitiesRepo = utilitiesRepo;
            _mapper = mapper;
            _passwordService = passwordService;
        }
        public async Task<CustomResponse<List<StudentErrorSheet>>> AddStudents(IFormFile file) //Code can be simplified by distributing the steps on mutliple functions
        {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };

            if (file == null || file.Length == 0)
                return new CustomResponse<List<StudentErrorSheet>>(400,"File must be specefied");
            

            string? fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (fileExtension == null)
                return new CustomResponse<List<StudentErrorSheet>>(400,"File does not have an extension");

            if (fileExtension != ".csv")
                return new CustomResponse<List<StudentErrorSheet>>(400, "The file specefied must be a csv file");

            using var reader = new StreamReader(file.OpenReadStream());

            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<StudentCSVMapper>(); //used to trim the strings coming from the csv file

            var studentsInputModel = new List<StudentsInputModelCSV>();

            var errorSheetList = new List<StudentErrorSheet>();

            int rowNumber = 2;
            await foreach (var student in csv.GetRecordsAsync<StudentsInputModelCSV>())
            {
                 StudentErrorSheet studentErrorSheet = new StudentErrorSheet(rowNumber);

                if (!_utilitiesRepo.IsValidEmail(student.Email))
                {
                    studentErrorSheet.Email = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (!_utilitiesRepo.IsValidPhoneNumber(student.Phone))
                {
                    studentErrorSheet.Phone = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (!_utilitiesRepo.IsValidNationalId(student.NationalNumber))
                {
                    studentErrorSheet.NationalNumber = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (student.FirstName == "") {
                    studentErrorSheet.FirstName = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (student.FathertName == "") {
                    studentErrorSheet.FathertName = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (student.GrandfatherName == "") {
                    studentErrorSheet.GrandfatherName = false;
                    studentErrorSheet.numberOfErrors++;
                } 
                if (student.LastName == "") {
                    studentErrorSheet.LastName = false;
                    studentErrorSheet.numberOfErrors++;
                }


                if(studentErrorSheet.numberOfErrors != 0)
                {
                    errorSheetList.Add(studentErrorSheet);
                }
                else
                {
                    student.Password = _passwordService.HashPassword(new Account(),student.PasswordAsString);
                    studentsInputModel.Add(student);
                }
                
                 rowNumber++; 
                Console.WriteLine($"{student.Email} {student.FirstName} {student.FathertName} {student.GrandfatherName} {student.LastName} {student.Phone} {student.NationalNumber} {student.Password}");
            }

            if (errorSheetList.Any())
                return new CustomResponse<List<StudentErrorSheet>>(400, "Invalid data please check the errorsheet for more info", errorSheetList);

            if (!studentsInputModel.Any())
                return new CustomResponse<List<StudentErrorSheet>>(404, "NO studetns were found in the csv file");

            using var transaction = await _context.Database.BeginTransactionAsync();

        
            try
            {
                List<Account> accounts = _mapper.Map<List<Account>>(studentsInputModel);

                await _context.Accounts.AddRangeAsync(accounts);
                await _context.SaveChangesAsync();

                var students = _mapper.Map<List<Student>>(studentsInputModel);

                for (int i = 0; i < students.Count; i++)
                {
                    students[i].AccountId = accounts[i].AccountId;
                }

                await _context.Students.AddRangeAsync(students);
                await _context.SaveChangesAsync();


                await transaction.CommitAsync();
                return new CustomResponse<List<StudentErrorSheet>>(201, "Students added successfully");
            }
            catch
            {
                await transaction.RollbackAsync();
                return new CustomResponse<List<StudentErrorSheet>>(500, "Internal server error");
            }
        } 
        public Task<CustomResponse<bool>> AddTeachingAssistances(IFormFile file)
        {
            throw new NotImplementedException();
        }
        public Task<CustomResponse<bool>> AddProfessors(IFormFile file)
        {
            throw new NotImplementedException();
        }
        public Task<CustomResponse<bool>> AddCourses(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}

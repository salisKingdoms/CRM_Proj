using Dapper;
//ing WebApi.Entities;
using WS_CRM.Helper;
using WS_CRM.Feature.Customer.dto;
using AutoMapper;
//ing BCrypt.Net;
using WS_CRM.Feature.Customer.Model;

namespace WS_CRM.Feature.Customer.dao
{
    public class CustomerDao :ICustomerDao
    {
        private ICustomerRepo _custRepository;
        private readonly IMapper _mapper;
        
        public CustomerDao(ICustomerRepo custRepo, IMapper mapper)
        {
            _custRepository = custRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Customers>> GetAll()
        {
            return await _custRepository.RepoGetAllCustomer();
        }
        public async Task CreateCustomer(CreateCustomerRequest request)
        {
             await _custRepository.CreateCustomer(request);
        }

        //public async Task<User> GetById(int id)
        //{
        //    using var connection = _context.CreateConnection();
        //    var sql = """
        //    SELECT * FROM Users 
        //    WHERE Id = @id
        //""";
        //    return await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
        //}

        //public async Task<User> GetByEmail(string email)
        //{
        //    using var connection = _context.CreateConnection();
        //    var sql = """
        //    SELECT * FROM Users 
        //    WHERE Email = @email
        //""";
        //    return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        //}

        //public async Task Create(User user)
        //{
        //    using var connection = _context.CreateConnection();
        //    var sql = """
        //    INSERT INTO Users (Title, FirstName, LastName, Email, Role, PasswordHash)
        //    VALUES (@Title, @FirstName, @LastName, @Email, @Role, @PasswordHash)
        //""";
        //    await connection.ExecuteAsync(sql, user);
        //}

        //public async Task Update(User user)
        //{
        //    using var connection = _context.CreateConnection();
        //    var sql = """
        //    UPDATE Users 
        //    SET Title = @Title,
        //        FirstName = @FirstName,
        //        LastName = @LastName, 
        //        Email = @Email, 
        //        Role = @Role, 
        //        PasswordHash = @PasswordHash
        //    WHERE Id = @Id
        //""";
        //    await connection.ExecuteAsync(sql, user);
        //}

        //public async Task Delete(int id)
        //{
        //    using var connection = _context.CreateConnection();
        //    var sql = """
        //    DELETE FROM Users 
        //    WHERE Id = @id
        //""";
        //    await connection.ExecuteAsync(sql, new { id });
        //}

    }
}

namespace UserService.Controller.Dto;

record UserResponse
{
    int Id { get; set; }
    string Login { get; set; }
    string Password { get; set; }
    string Name { get; set; }
    string Surname { get; set; }
    int Age { get; set; }
    
}
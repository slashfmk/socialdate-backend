namespace socialbackend.interfaces;

using socialbackend.Entities;

public interface ITokenService
{

    string CreateToken(AppUser user);
    
}
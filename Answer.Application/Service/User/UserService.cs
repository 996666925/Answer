using Answer.Application.Enum;
using Answer.Application.Service.User.Dto;


namespace Answer.Application.Service.User;

/// <summary>
/// 用户服务
/// </summary>
public class UserService(Repository<Entity.User> userRepository) : IDynamicApiController
{
    public async Task<string> PostRegister([Required] UserInput input)
    {
        var user = await userRepository.GetFirstAsync(u => u.Account == input.Account);

        if (user != null)
        {
            throw Oops.Oh(ErrorCodeEnum.D0004);
        }

        input.Password = MD5Encryption.Encrypt(input.Password);
        await userRepository.InsertReturnSnowflakeIdAsync(input.Adapt<Entity.User>());


        return UserConst.RegisterOk;
    }
}
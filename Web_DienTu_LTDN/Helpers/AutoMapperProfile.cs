using AutoMapper;
using Web_DienTu_LTDN.Data;
using Web_DienTu_LTDN.ViewModels;
namespace Web_DienTu_LTDN.Helpers
{
    public class AutoMapperProfile : Profile
    {   
            public AutoMapperProfile()
            {
                CreateMap<RegisterVM, KhachHang>();
                //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
                //.ReverseMap();
            }       
    }
}

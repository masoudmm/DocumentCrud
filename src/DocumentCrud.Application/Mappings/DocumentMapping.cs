using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.Entities;

namespace DocumentCrud.Application.Features.Mappings;

public class DocumentMapping : Profile
{
    public DocumentMapping()
    {
        CreateMap<Invoice, DocumentDto>()
            .ForMember(c => c.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(c => c.Number, opt => opt.MapFrom(s => s.Number))
            .ForMember(c => c.ExternalNumber, opt => opt.MapFrom(s => s.ExternalInvoiceNumber))
            .ForMember(c => c.Status, opt => opt.MapFrom(s => s.Status))
            .ForMember(c => c.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount));

        CreateMap<DocumentDto, Invoice>()
            .ForMember(c => c.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(c => c.Number, opt => opt.MapFrom(s => s.Number))
            .ForMember(c => c.ExternalInvoiceNumber, opt => opt.MapFrom(s => s.ExternalNumber))
            .ForMember(c => c.Status, opt => opt.MapFrom(s => s.Status))
            .ForMember(c => c.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount));

        CreateMap<IndependentCreditNote, DocumentDto>()
            .ForMember(c => c.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(c => c.Number, opt => opt.MapFrom(s => s.Number))
            .ForMember(c => c.ExternalNumber, opt => opt.MapFrom(s => s.ExternalCreditNumber))
            .ForMember(c => c.Status, opt => opt.MapFrom(s => s.Status))
            .ForMember(c => c.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount));

        CreateMap<DocumentDto, IndependentCreditNote>()
            .ForMember(c => c.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(c => c.Number, opt => opt.MapFrom(s => s.Number))
            .ForMember(c => c.ExternalCreditNumber, opt => opt.MapFrom(s => s.ExternalNumber))
            .ForMember(c => c.Status, opt => opt.MapFrom(s => s.Status))
            .ForMember(c => c.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount));
    }
}

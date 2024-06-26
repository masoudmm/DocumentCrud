﻿using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.CreditAggregate;
using DocumentCrud.Domain.InvoiceAggregate;

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
            .ForMember(c => c.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount))
            .ForMember(c => c.Type, opt => opt.MapFrom(s => DocumentType.Invoice));

        CreateMap<IndependentCreditNote, DocumentDto>()
            .ForMember(c => c.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(c => c.Number, opt => opt.MapFrom(s => s.Number))
            .ForMember(c => c.ExternalNumber, opt => opt.MapFrom(s => s.ExternalCreditNumber))
            .ForMember(c => c.Status, opt => opt.MapFrom(s => s.Status))
            .ForMember(c => c.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount))
            .ForMember(c => c.Type, opt => opt.MapFrom(s => DocumentType.IndependentCredit));


        CreateMap<DependentCreditNoteDto, DependentCreditNote>()
            .ForMember(c => c.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(c => c.Number, opt => opt.MapFrom(s => s.Number))
            .ForMember(c => c.ExternalCreditNumber, opt => opt.MapFrom(s => s.ExternalCreditNumber))
            .ForMember(c => c.Status, opt => opt.MapFrom(s => s.Status))
            .ForMember(c => c.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount));

        CreateMap<DependentCreditNote, DependentCreditNoteDto>()
            .ForMember(c => c.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(c => c.Number, opt => opt.MapFrom(s => s.Number))
            .ForMember(c => c.ExternalCreditNumber, opt => opt.MapFrom(s => s.ExternalCreditNumber))
            .ForMember(c => c.Status, opt => opt.MapFrom(s => s.Status))
            .ForMember(c => c.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount));
    }
}

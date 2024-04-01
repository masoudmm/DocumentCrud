﻿namespace DocumentCrud.Domain.Entities;

public class IndependentCreditNote : AccountingDocument
{
    private IndependentCreditNote(string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        Number = number;
        ExternalNumber = externalNumber;
        Status = status;
        TotalAmount = totalAmount;
    }

    public void Edit(string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        //We can check some business rules and create events here

        Number = number;
        ExternalNumber = externalNumber;
        Status = status;
        TotalAmount = totalAmount;
    }

    public static IndependentCreditNote CreateNewInvoice(string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        //We can check some business rules and create events here

        return new IndependentCreditNote(number,
        externalNumber,
        status,
        totalAmount);
    }
}

using System;
using System.Collections.Generic;

namespace Bab.BatchData.Models;

public partial class Contact
{
    public Guid Id { get; set; }

    public DateTime BatchDate { get; set; }

    public string? CustomerNameArabic { get; set; }

    public string? CustomerNameEnglish { get; set; }

    public string? Nationality { get; set; }

    public decimal? IdType { get; set; }

    public string? Employer { get; set; }

    public string? Gender { get; set; }

    public DateTime? BirthdateGregorian { get; set; }

    public string? MaritalStatus { get; set; }

    public decimal? BirthdateHijri { get; set; }

    public decimal? PreferredLanguage { get; set; }

    public string? LegalStatusCode { get; set; }

    public string? LegalStatusDescription { get; set; }

    public decimal? Cif { get; set; }

    public decimal? Segment { get; set; }

    public string? Sector { get; set; }

    public decimal? CustRisk { get; set; }

    public string? SamaClass { get; set; }

    public string? City { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? GrandfatherName { get; set; }

    public string? FamilyName { get; set; }

    public string? FirstNameEn { get; set; }

    public string? MiddleNameEn { get; set; }

    public string? GrandfatherNameEn { get; set; }

    public string? FamilyNameEn { get; set; }

    public decimal? MobileNumber { get; set; }

    public string? Email { get; set; }

    public string? SegmentDescription { get; set; }

    public string? LegalId { get; set; }
}

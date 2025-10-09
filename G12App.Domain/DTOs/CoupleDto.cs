namespace G12App.Domain.DTOs;

public class CoupleDto
{
    public MemberDtoSmall Husband { get; set; }
    public MemberDtoSmall Wife { get; set; }

    public bool HasHusband => Husband != null && Husband.Id.HasValue && Husband.Id.Value > 0;
    public bool HasWife => Wife != null && Wife.Id.HasValue && Wife.Id.Value > 0;

    public bool IsComplete => HasHusband && HasWife;
}

using FluentValidation;
using ON.Mercury.Service.Models.Channels;

namespace ON.Mercury.Service.Validators
{
    public sealed class CreateOrUpdateChannelValidator : AbstractValidator<CreateOrUpdateChannel>
    {
        public CreateOrUpdateChannelValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Channel Name is Required");
            RuleFor(c => c.Name).Length(50).WithMessage("Channel Name Must Not Exceed 50 Characters");
            RuleFor(c => c.Category).Length(0, 50).WithMessage("Category Must Not Exceed 50 Characters");
            RuleFor(c => c.Description).Length(0, 255).WithMessage("Description Must Not Exceed 255 Characters");
        }
    }
}

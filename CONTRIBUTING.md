# Contributing to TR.Maui.AnchorPopover

Thank you for your interest in contributing to TR.Maui.AnchorPopover! We welcome contributions from the community.

## Getting Started

1. Fork the repository
2. Clone your fork locally
3. Create a new branch for your feature or bug fix
4. Make your changes
5. Test your changes thoroughly
6. Submit a pull request

## Development Setup

### Prerequisites

- .NET 10.0 SDK or later
- Visual Studio 2022 (Windows) or Visual Studio Code with C# extension
- .NET MAUI workload installed:
  ```bash
  dotnet workload install maui-android
  ```

### Building the Project

```bash
# Restore dependencies
dotnet restore

# Build the library
dotnet build src/TR.Maui.AnchorPopover/TR.Maui.AnchorPopover.csproj

# Build the sample application
dotnet build samples/TR.Maui.AnchorPopover.Sample/TR.Maui.AnchorPopover.Sample.csproj
```

## Code Style

- Follow the existing code style in the project
- Use meaningful variable and method names
- Add XML documentation comments to public APIs
- Keep methods focused and concise

## Commit Messages

- Use clear and descriptive commit messages
- Start with a verb in present tense (e.g., "Add", "Fix", "Update")
- Reference issue numbers when applicable

## Pull Request Process

1. Ensure your code builds without errors
2. Update the README.md if you're adding new features
3. Add or update documentation as needed
4. Ensure all existing tests pass (when tests are available)
5. Update the CHANGELOG.md with your changes
6. Submit your pull request with a clear description of the changes

## Reporting Bugs

When reporting bugs, please include:
- A clear description of the issue
- Steps to reproduce the problem
- Expected behavior vs actual behavior
- Platform and version information
- Code samples or screenshots if applicable

## Feature Requests

We welcome feature requests! Please:
- Check if the feature has already been requested
- Provide a clear description of the feature
- Explain the use case and benefits
- Be open to discussion and feedback

## Code of Conduct

- Be respectful and inclusive
- Welcome newcomers and help them get started
- Focus on constructive feedback
- Maintain a professional and friendly environment

## Questions?

If you have questions, feel free to:
- Open an issue for discussion
- Check existing issues and pull requests
- Contact the maintainers

Thank you for contributing to TR.Maui.AnchorPopover!

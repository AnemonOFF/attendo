# GitHub Actions Workflows

This repository includes several GitHub Actions workflows to automate CI/CD processes for the Attendo project.

## Workflows Overview

### 1. CI/CD Pipeline (`.github/workflows/ci.yml`)

**Triggers:** Push to main/master/develop branches, Pull Requests

**Jobs:**

- **Frontend Lint & Build**: Runs ESLint, TypeScript type checking, and builds the React frontend
- **Backend Build & Test**: Builds the ASP.NET Core API and runs tests
- **Docker Build**: Builds Docker images for both frontend and backend
- **Security Scan**: Runs Trivy vulnerability scanner

### 2. Code Quality (`.github/workflows/code-quality.yml`)

**Triggers:** Push to main/master/develop branches, Pull Requests

**Jobs:**

- **Frontend Code Quality**: Prettier formatting check, detailed ESLint analysis
- **Backend Code Quality**: .NET format check, analyzer runs
- **Dependency Check**: Security audit for npm and .NET packages

### 3. Pull Request Checks (`.github/workflows/pr-checks.yml`)

**Triggers:** Pull Request events (opened, synchronize, reopened)

**Features:**

- Comprehensive frontend and backend validation
- TODO/FIXME comment detection
- Large file size warnings
- PR summary generation

### 4. Deploy (`.github/workflows/deploy.yml`)

**Triggers:** Push to main/master branches, Manual dispatch

**Jobs:**

- **Deploy to Staging**: Builds and pushes Docker images, deploys to staging
- **Deploy to Production**: Production deployment (customize as needed)

## Required Secrets

To use the deployment workflow, configure these secrets in your repository:

- `DOCKER_USERNAME`: Docker Hub username
- `DOCKER_PASSWORD`: Docker Hub password or access token

## Workflow Features

### Frontend (React + Vite + TypeScript)

- ✅ ESLint with comprehensive rules
- ✅ TypeScript type checking
- ✅ Prettier formatting validation
- ✅ Build verification
- ✅ Dependency security audit

### Backend (ASP.NET Core 9.0)

- ✅ .NET build and restore
- ✅ Unit test execution
- ✅ Code formatting validation
- ✅ Security vulnerability scanning
- ✅ Package dependency checks

### Docker

- ✅ Multi-stage builds
- ✅ Build caching for performance
- ✅ Image security scanning
- ✅ Automated tagging

### Security

- ✅ Trivy vulnerability scanning
- ✅ npm audit for frontend dependencies
- ✅ .NET package vulnerability checks
- ✅ SARIF report upload to GitHub Security tab

## Customization

### Adding New Checks

1. Edit the relevant workflow file
2. Add new steps to the appropriate job
3. Configure triggers as needed

### Environment-Specific Deployments

1. Update `.github/workflows/deploy.yml`
2. Add environment-specific secrets
3. Configure deployment targets

### Performance Optimization

- Workflows use caching for dependencies
- Docker builds use GitHub Actions cache
- Parallel job execution where possible

## Troubleshooting

### Common Issues

1. **ESLint failures**: Check `front/eslint.config.js` for rule configuration
2. **Build failures**: Verify all dependencies are properly installed
3. **Docker build issues**: Check Dockerfile syntax and base images
4. **Secret errors**: Ensure all required secrets are configured

### Debugging

- Check workflow logs in the Actions tab
- Use `act` locally to test workflows
- Validate YAML syntax with online tools

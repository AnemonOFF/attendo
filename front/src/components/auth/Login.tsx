import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";

import logo from "../../assets/logo.png";
import { useLogin } from "../../hooks/useAuth";

interface LoginFormData {
  email: string;
  password: string;
  rememberMe: boolean;
}

interface FormErrors {
  email?: string;
  password?: string;
}

const Login: React.FC = () => {
  const navigate = useNavigate();
  const loginMutation = useLogin();
  // Uncomment to use your custom auth hook
  // const { loginMutation } = useAuth();

  const [formData, setFormData] = useState<LoginFormData>({
    email: "",
    password: "",
    rememberMe: false,
  });

  const [errors, setErrors] = useState<FormErrors>({});
  const [focusedField, setFocusedField] = useState<string>("");
  const [_, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState("");

  // Theme colors
  const theme = {
    primary: "#4A90E2",
    primaryHover: "#3A7BC8",
    background: "#F8F9FA",
    white: "#FFFFFF",
    error: "#dc3545",
    text: {
      primary: "#212529",
      secondary: "#6C757D",
      light: "#FFFFFF",
    },
    border: "#DEE2E6",
  };

  // Validation function
  const validateForm = (): boolean => {
    const newErrors: FormErrors = {};

    // Email validation
    if (!formData.email) {
      newErrors.email = "Email is required";
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = "Please enter a valid email address";
    }

    // Password validation
    if (!formData.password) {
      newErrors.password = "Password is required";
    } else if (formData.password.length < 6) {
      newErrors.password = "Password must be at least 6 characters";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Handle form submission
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setApiError("");

    if (!validateForm()) return;

    setIsLoading(true);

    try {
      // // Use the loginMutation with correct field names
      // await loginMutation.mutateAsync({
      // 	login: formData.email, // ← Map email to login
      // 	password: formData.password,
      // });

      // // Store remember me preference if needed
      // if (formData.rememberMe) {
      // 	localStorage.setItem('rememberMe', 'true');
      // }

      // // On successful login, navigate to dashboard
      console.warn("Login successful");
      navigate("/dashboard"); // ← Change this to your dashboard route

      // navigate('/dashboard');
    } catch (error: unknown) {
      // Type guard to safely access error properties
      if (error instanceof Error) {
        setApiError(error.message);
      } else if (
        typeof error === "object" &&
        error !== null &&
        "response" in error
      ) {
        // Handle Axios-style errors
        const axiosError = error as {
          response?: { data?: { message?: string } };
        };
        setApiError(
          axiosError.response?.data?.message ||
            "Invalid email or password. Please try again.",
        );
      } else {
        setApiError("Invalid email or password. Please try again.");
      }
    }
  };

  // Handle input changes
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));

    // Clear field error on change
    if (errors[name as keyof FormErrors]) {
      setErrors((prev) => ({ ...prev, [name]: undefined }));
    }

    // Clear API error on change
    setApiError("");
  };

  // Styles
  const styles = {
    container: {
      minHeight: "100vh",
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      backgroundColor: theme.background,
      padding: "20px",
    },
    card: {
      backgroundColor: theme.white,
      borderRadius: "16px",
      boxShadow: "0 4px 6px rgba(0, 0, 0, 0.07)",
      padding: "40px",
      width: "100%",
      maxWidth: "440px",
    },
    logo: {
      display: "block",
      width: "120px",
      height: "auto",
      margin: "0 auto 8px auto",
    },
    title: {
      fontSize: "40px",
      color: theme.primary,
      textAlign: "center" as const,
      marginBottom: "32px",
      fontWeight: "700",
    },
    subtitle: {
      fontSize: "25px",
      color: theme.primary,
      textAlign: "center" as const,
      marginBottom: "32px",
      fontWeight: "bold",
    },
    inputGroup: {
      marginBottom: "20px",
    },
    label: {
      display: "block",
      fontSize: "14px",
      fontWeight: "500",
      color: theme.text.primary,
      marginBottom: "8px",
    },
    input: {
      width: "100%",
      padding: "12px 16px",
      fontSize: "14px",
      border: `1px solid ${theme.border}`,
      borderRadius: "8px",
      outline: "none",
      transition: "all 0.2s ease",
      backgroundColor: theme.white,
      color: theme.text.primary,
      boxSizing: "border-box" as const,
    },
    inputFocus: {
      borderColor: theme.primary,
      boxShadow: `0 0 0 3px ${theme.primary}20`,
    },
    inputError: {
      borderColor: theme.error,
    },
    button: {
      width: "100%",
      padding: "14px",
      fontSize: "15px",
      fontWeight: "600",
      color: theme.text.light,
      backgroundColor: theme.primary,
      border: "none",
      borderRadius: "8px",
      cursor: "pointer",
      transition: "all 0.2s ease",
      outline: "none",
    },
    buttonDisabled: {
      opacity: 0.6,
      cursor: "not-allowed",
    },
    checkboxContainer: {
      display: "flex",
      alignItems: "center",
      gap: "8px",
    },
    checkbox: {
      width: "18px",
      height: "18px",
      cursor: "pointer",
      accentColor: theme.primary,
    },
    checkboxLabel: {
      fontSize: "14px",
      color: theme.text.primary,
      cursor: "pointer",
      userSelect: "none" as const,
    },
    link: {
      color: theme.primary,
      textDecoration: "none",
      fontSize: "14px",
      fontWeight: "500",
      transition: "opacity 0.2s ease",
    },
    textCenter: {
      textAlign: "center" as const,
      marginTop: "24px",
      fontSize: "14px",
      color: theme.text.secondary,
    },
    error: {
      color: theme.error,
      fontSize: "13px",
      marginTop: "6px",
    },
    apiError: {
      color: theme.error,
      fontSize: "14px",
      marginBottom: "20px",
      textAlign: "center" as const,
      padding: "12px",
      backgroundColor: `${theme.error}10`,
      borderRadius: "8px",
    },
  };

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <div style={styles.title}>ATTENDO</div>
        <img src={logo} alt="Attendo Logo" style={styles.logo} />
        <div style={styles.subtitle}>Log In</div>

        {apiError && (
          <div style={styles.apiError} role="alert">
            {apiError}
          </div>
        )}

        <form onSubmit={handleSubmit} noValidate>
          {/* Email Field */}
          <div style={styles.inputGroup}>
            <label htmlFor="email" style={styles.label}>
              Email Address
            </label>
            <input
              id="email"
              name="email"
              type="email"
              autoComplete="email"
              value={formData.email}
              onChange={handleChange}
              onFocus={() => setFocusedField("email")}
              onBlur={() => setFocusedField("")}
              style={{
                ...styles.input,
                ...(focusedField === "email" ? styles.inputFocus : {}),
                ...(errors.email ? styles.inputError : {}),
              }}
              placeholder="Enter your email"
              aria-invalid={!!errors.email}
              aria-describedby={errors.email ? "email-error" : undefined}
              disabled={loginMutation.isPending}
            />
            {errors.email && (
              <div id="email-error" style={styles.error} role="alert">
                {errors.email}
              </div>
            )}
          </div>

          {/* Password Field */}
          <div style={styles.inputGroup}>
            <label htmlFor="password" style={styles.label}>
              Password
            </label>
            <input
              id="password"
              name="password"
              type="password"
              autoComplete="current-password"
              value={formData.password}
              onChange={handleChange}
              onFocus={() => setFocusedField("password")}
              onBlur={() => setFocusedField("")}
              style={{
                ...styles.input,
                ...(focusedField === "password" ? styles.inputFocus : {}),
                ...(errors.password ? styles.inputError : {}),
              }}
              placeholder="Enter your password"
              aria-invalid={!!errors.password}
              aria-describedby={errors.password ? "password-error" : undefined}
              disabled={loginMutation.isPending}
            />
            {errors.password && (
              <div id="password-error" style={styles.error} role="alert">
                {errors.password}
              </div>
            )}
          </div>

          {/* Remember Me & Forgot Password */}
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              marginBottom: "24px",
            }}
          >
            <div style={styles.checkboxContainer}>
              <input
                id="rememberMe"
                name="rememberMe"
                type="checkbox"
                checked={formData.rememberMe}
                onChange={handleChange}
                style={styles.checkbox}
                disabled={loginMutation.isPending}
                aria-label="Remember me"
              />
              <label htmlFor="rememberMe" style={styles.checkboxLabel}>
                Remember me
              </label>
            </div>
            <Link
              to="/recover"
              style={styles.link}
              onMouseEnter={(e) => {
                (e.target as HTMLAnchorElement).style.opacity = "0.8";
              }}
              onMouseLeave={(e) => {
                (e.target as HTMLAnchorElement).style.opacity = "1";
              }}
            >
              Forgot password?
            </Link>
          </div>

          {/* Submit Button */}
          <button
            type="submit"
            disabled={loginMutation.isPending} // ← Use isPending from mutation
            style={{
              ...styles.button,
              ...(loginMutation.isPending ? styles.buttonDisabled : {}),
            }}
            onMouseEnter={(e) => {
              if (!loginMutation.isPending) {
                (e.target as HTMLButtonElement).style.backgroundColor =
                  theme.primaryHover;
              }
            }}
            onMouseLeave={(e) => {
              (e.target as HTMLButtonElement).style.backgroundColor =
                theme.primary;
            }}
            aria-busy={loginMutation.isPending}
          >
            {loginMutation.isPending ? "Signing in..." : "Sign In"}
          </button>
        </form>

        {/* Register Link */}
        <div style={styles.textCenter}>
          Don't have an account?{" "}
          <Link
            to="/register"
            style={styles.link}
            onMouseEnter={(e) => {
              (e.target as HTMLAnchorElement).style.opacity = "0.8";
            }}
            onMouseLeave={(e) => {
              (e.target as HTMLAnchorElement).style.opacity = "1";
            }}
          >
            Sign up
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Login;

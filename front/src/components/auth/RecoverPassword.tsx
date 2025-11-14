import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";

// Types
interface RecoverFormData {
  email: string;
}

// Theme colors
const theme = {
  primary: "#4A90E2",
  secondary: "#6C757D",
  background: "#F8F9FA",
  white: "#FFFFFF",
  error: "#dc3545",
  success: "#28a745",
  text: {
    primary: "#212529",
    secondary: "#6C757D",
    light: "#FFFFFF",
  },
  border: "#DEE2E6",
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
    transition: "all 0.3s ease",
  },
  logo: {
    fontSize: "32px",
    fontWeight: "700" as const,
    color: theme.primary,
    textAlign: "center" as const,
    marginBottom: "8px",
    letterSpacing: "-0.5px",
  },
  subtitle: {
    fontSize: "14px",
    color: theme.text.secondary,
    textAlign: "center" as const,
    marginBottom: "32px",
    lineHeight: "1.5",
  },
  inputGroup: {
    marginBottom: "20px",
  },
  label: {
    display: "block",
    fontSize: "14px",
    fontWeight: "500" as const,
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
  inputDisabled: {
    backgroundColor: "#f5f5f5",
    cursor: "not-allowed",
    opacity: 0.7,
  },
  button: {
    width: "100%",
    padding: "14px",
    fontSize: "15px",
    fontWeight: "600" as const,
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
  error: {
    color: theme.error,
    fontSize: "13px",
    marginTop: "6px",
  },
  success: {
    color: theme.success,
    fontSize: "14px",
    padding: "16px",
    backgroundColor: `${theme.success}15`,
    border: `1px solid ${theme.success}40`,
    borderRadius: "8px",
    marginBottom: "20px",
    textAlign: "center" as const,
    lineHeight: "1.6",
  },
  successIcon: {
    fontSize: "18px",
    marginRight: "8px",
  },
  textCenter: {
    textAlign: "center" as const,
    marginTop: "24px",
    fontSize: "14px",
    color: theme.text.secondary,
  },
  link: {
    color: theme.primary,
    textDecoration: "none",
    fontSize: "14px",
    fontWeight: "500" as const,
    transition: "opacity 0.2s ease",
  },
  description: {
    fontSize: "14px",
    color: theme.text.secondary,
    textAlign: "center" as const,
    marginBottom: "24px",
    lineHeight: "1.6",
  },
};

const RecoverPassword: React.FC = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<RecoverFormData>({
    email: "",
  });
  const [errors, setErrors] = useState<Partial<RecoverFormData>>({});
  const [focusedField, setFocusedField] = useState<string>("");
  const [isLoading, setIsLoading] = useState(false);
  const [success, setSuccess] = useState(false);

  const validateForm = (): boolean => {
    const newErrors: Partial<RecoverFormData> = {};

    if (!formData.email) {
      newErrors.email = "Email is required";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = "Please enter a valid email address";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) return;

    setIsLoading(true);

    try {
      // TODO: Replace with actual API call
      // Example: await authController.recoverPassword({ email: formData.email });

      // Simulate API call
      await new Promise((resolve) => setTimeout(resolve, 1500));

      setSuccess(true);

      // Auto-redirect to login after 3 seconds
      setTimeout(() => {
        navigate("/login");
      }, 3000);
    } catch {
      setErrors({
        email:
          "Failed to send recovery email. Please verify your email address and try again.",
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));

    // Clear errors when user starts typing
    if (errors[name as keyof RecoverFormData]) {
      setErrors((prev) => ({ ...prev, [name]: undefined }));
    }

    // Reset success state if user modifies email after success
    if (success) {
      setSuccess(false);
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <div style={styles.logo}>Attendo</div>
        <div style={styles.subtitle}>Reset your password</div>

        <div style={styles.description}>
          Enter your email address and we'll send you instructions to reset your
          password.
        </div>

        {success && (
          <div style={styles.success} role="alert">
            <span style={styles.successIcon}>✓</span>
            Recovery email sent successfully! Check your inbox for instructions.
            <br />
            Redirecting to login...
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div style={styles.inputGroup}>
            <label htmlFor="email" style={styles.label}>
              Email Address
            </label>
            <input
              id="email"
              name="email"
              type="email"
              value={formData.email}
              onChange={handleChange}
              onFocus={() => setFocusedField("email")}
              onBlur={() => setFocusedField("")}
              style={{
                ...styles.input,
                ...(focusedField === "email" ? styles.inputFocus : {}),
                ...(errors.email ? styles.inputError : {}),
                ...(success ? styles.inputDisabled : {}),
              }}
              placeholder="Enter your email address"
              aria-invalid={!!errors.email}
              aria-describedby={errors.email ? "email-error" : undefined}
              disabled={success || isLoading}
              autoComplete="email"
              autoFocus
            />
            {errors.email && (
              <div id="email-error" style={styles.error} role="alert">
                {errors.email}
              </div>
            )}
          </div>

          <button
            type="submit"
            disabled={isLoading || success}
            style={{
              ...styles.button,
              ...(isLoading || success ? styles.buttonDisabled : {}),
            }}
            onMouseEnter={(e) => {
              if (!isLoading && !success) {
                (e.target as HTMLButtonElement).style.backgroundColor =
                  "#3A7BC8";
              }
            }}
            onMouseLeave={(e) => {
              if (!isLoading && !success) {
                (e.target as HTMLButtonElement).style.backgroundColor =
                  theme.primary;
              }
            }}
            aria-busy={isLoading}
          >
            {isLoading
              ? "Sending..."
              : success
                ? "Email Sent!"
                : "Send Recovery Email"}
          </button>
        </form>

        <div style={styles.textCenter}>
          <Link
            to="/login"
            style={styles.link}
            onMouseEnter={(e) => {
              (e.target as HTMLAnchorElement).style.opacity = "0.8";
            }}
            onMouseLeave={(e) => {
              (e.target as HTMLAnchorElement).style.opacity = "1";
            }}
          >
            ← Back to Sign In
          </Link>
        </div>
      </div>
    </div>
  );
};

export default RecoverPassword;

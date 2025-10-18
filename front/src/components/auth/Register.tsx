import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

import logo from '../../assets/logo.png';
import { useRegister } from '../../hooks/useAuth'; // ← Add this import

// Types
interface RegisterFormData {
	name: string;
	email: string;
	password: string;
	confirmPassword: string;
	acceptTerms: boolean;
}

// Theme colors - matching Login screen
const theme = {
	primary: '#4A90E2',
	secondary: '#6C757D',
	background: '#F8F9FA',
	white: '#FFFFFF',
	error: '#dc3545',
	text: {
		primary: '#212529',
		secondary: '#6C757D',
		light: '#FFFFFF',
	},
	border: '#DEE2E6',
};

// Styles
const styles = {
	container: {
		minHeight: '100vh',
		display: 'flex',
		alignItems: 'center',
		justifyContent: 'center',
		backgroundColor: theme.background,
		padding: '20px',
	} as React.CSSProperties,
	card: {
		backgroundColor: theme.white,
		borderRadius: '16px',
		boxShadow: '0 4px 6px rgba(0, 0, 0, 0.07)',
		padding: '40px',
		width: '100%',
		maxWidth: '440px',
		transition: 'all 0.3s ease',
	} as React.CSSProperties,
	logo: {
		display: 'block',
		width: '120px',
		height: 'auto',
		margin: '0 auto 8px auto',
	} as React.CSSProperties,
	title: {
		fontSize: '40px',
		color: theme.primary,
		textAlign: 'center' as const,
		marginBottom: '32px',
		fontWeight: '700',
	} as React.CSSProperties,
	subtitle: {
		fontSize: '25px',
		color: theme.primary,
		textAlign: 'center' as const,
		marginBottom: '32px',
		fontWeight: 'bold',
	} as React.CSSProperties,
	inputGroup: {
		marginBottom: '20px',
	} as React.CSSProperties,
	label: {
		display: 'block',
		fontSize: '14px',
		fontWeight: '500',
		color: theme.text.primary,
		marginBottom: '8px',
	} as React.CSSProperties,
	input: {
		width: '100%',
		padding: '12px 16px',
		fontSize: '14px',
		border: `1px solid ${theme.border}`,
		borderRadius: '8px',
		outline: 'none',
		transition: 'all 0.2s ease',
		backgroundColor: theme.white,
		color: theme.text.primary,
		boxSizing: 'border-box',
	} as React.CSSProperties,
	inputFocus: {
		borderColor: theme.primary,
		boxShadow: `0 0 0 3px ${theme.primary}20`,
	} as React.CSSProperties,
	inputError: {
		borderColor: theme.error,
	} as React.CSSProperties,
	button: {
		width: '100%',
		padding: '14px',
		fontSize: '15px',
		fontWeight: '600',
		color: theme.text.light,
		backgroundColor: theme.primary,
		border: 'none',
		borderRadius: '8px',
		cursor: 'pointer',
		transition: 'all 0.2s ease',
		outline: 'none',
	} as React.CSSProperties,
	buttonDisabled: {
		opacity: 0.6,
		cursor: 'not-allowed',
	} as React.CSSProperties,
	checkboxContainer: {
		display: 'flex',
		alignItems: 'center',
		gap: '8px',
	} as React.CSSProperties,
	checkbox: {
		width: '18px',
		height: '18px',
		cursor: 'pointer',
		accentColor: theme.primary,
	} as React.CSSProperties,
	checkboxLabel: {
		fontSize: '14px',
		color: theme.text.primary,
		cursor: 'pointer',
		userSelect: 'none',
	} as React.CSSProperties,
	link: {
		color: theme.primary,
		textDecoration: 'none',
		fontSize: '14px',
		fontWeight: '500',
		transition: 'opacity 0.2s ease',
	} as React.CSSProperties,
	textCenter: {
		textAlign: 'center',
		marginTop: '24px',
		fontSize: '14px',
		color: theme.text.secondary,
	} as React.CSSProperties,
	error: {
		color: theme.error,
		fontSize: '13px',
		marginTop: '6px',
	} as React.CSSProperties,
};

const Register: React.FC = () => {
	const navigate = useNavigate();
	const registerMutation = useRegister(); // ← Add this hook

	const [formData, setFormData] = useState<RegisterFormData>({
		name: '',
		email: '',
		password: '',
		confirmPassword: '',
		acceptTerms: false,
	});

	const [errors, setErrors] = useState<Partial<Record<keyof RegisterFormData, string>>>({});
	const [focusedField, setFocusedField] = useState<string>('');
	const [isLoading, setIsLoading] = useState(false);
	const [apiError, setApiError] = useState('');

	const validateForm = (): boolean => {
		const newErrors: Partial<Record<keyof RegisterFormData, string>> = {};

		// Name validation
		if (!formData.name.trim()) {
			newErrors.name = 'Name is required';
		} else if (formData.name.trim().length < 2) {
			newErrors.name = 'Name must be at least 2 characters';
		}

		// Email validation
		if (!formData.email) {
			newErrors.email = 'Email is required';
		} else if (!/\S+@\S+\.\S+/.test(formData.email)) {
			newErrors.email = 'Please enter a valid email address';
		}

		// Password validation
		if (!formData.password) {
			newErrors.password = 'Password is required';
		} else if (formData.password.length < 6) {
			newErrors.password = 'Password must be at least 6 characters';
		}

		// Confirm password validation
		if (!formData.confirmPassword) {
			newErrors.confirmPassword = 'Please confirm your password';
		} else if (formData.password !== formData.confirmPassword) {
			newErrors.confirmPassword = 'Passwords do not match';
		}

		// Terms acceptance validation
		if (!formData.acceptTerms) {
			newErrors.acceptTerms = 'You must accept the terms and conditions';
		}

		setErrors(newErrors);
		return Object.keys(newErrors).length === 0;
	};

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		setApiError('');

		if (!validateForm()) return;

		setIsLoading(true);

		try {
			// Use the registerMutation with correct field names
			await registerMutation.mutateAsync({
				name: formData.name,
				login: formData.email, // ← Map email to login
				password: formData.password,
			});

			console.warn('Registration successful');

			// Navigate to login page after successful registration
			navigate('/login');
		} catch (error: unknown) {
			// Properly handle the error with type checking
			if (error instanceof Error) {
				setApiError(error.message || 'Registration failed. Please try again.');
			} else if (typeof error === 'object' && error !== null && 'response' in error) {
				// Handle Axios errors
				const axiosError = error as { response?: { data?: { message?: string } } };
				setApiError(
					axiosError.response?.data?.message || 'Registration failed. Please try again.'
				);
			} else {
				setApiError('Registration failed. Please try again.');
			}
		} finally {
			setIsLoading(false);
		}
	};

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const { name, value, type, checked } = e.target;

		setFormData(prev => ({
			...prev,
			[name]: type === 'checkbox' ? checked : value,
		}));

		// Clear field-specific error when user starts typing
		if (errors[name as keyof RegisterFormData]) {
			setErrors(prev => ({ ...prev, [name]: undefined }));
		}

		// Clear API error when user makes changes
		if (apiError) {
			setApiError('');
		}
	};

	const handleFocus = (fieldName: string) => {
		setFocusedField(fieldName);
	};

	const handleBlur = () => {
		setFocusedField('');
	};

	const getInputStyle = (fieldName: keyof RegisterFormData) => ({
		...styles.input,
		...(focusedField === fieldName ? styles.inputFocus : {}),
		...(errors[fieldName] ? styles.inputError : {}),
	});

	return (
		<div style={styles.container}>
			<div style={styles.card}>
				<div style={styles.title}>ATTENDO</div>
				<img src={logo} alt="Attendo Logo" style={styles.logo} />
				<div style={styles.subtitle}>Create your account</div>

				{apiError && (
					<div
						style={{
							...styles.error,
							marginBottom: '20px',
							textAlign: 'center',
							padding: '12px',
							backgroundColor: '#f8d7da',
							borderRadius: '8px',
						}}
						role="alert"
					>
						{apiError}
					</div>
				)}

				<form onSubmit={handleSubmit} noValidate>
					{/* Full Name Field */}
					<div style={styles.inputGroup}>
						<label htmlFor="name" style={styles.label}>
							Full Name
						</label>
						<input
							id="name"
							name="name"
							type="text"
							value={formData.name}
							onChange={handleChange}
							onFocus={() => handleFocus('name')}
							onBlur={handleBlur}
							style={getInputStyle('name')}
							placeholder="Enter your full name"
							aria-invalid={!!errors.name}
							aria-describedby={errors.name ? 'name-error' : undefined}
							disabled={registerMutation.isPending}
						/>
						{errors.name && (
							<div id="name-error" style={styles.error} role="alert">
								{errors.name}
							</div>
						)}
					</div>

					{/* Email Field */}
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
							onFocus={() => handleFocus('email')}
							onBlur={handleBlur}
							style={getInputStyle('email')}
							placeholder="Enter your email"
							aria-invalid={!!errors.email}
							aria-describedby={errors.email ? 'email-error' : undefined}
							disabled={registerMutation.isPending}
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
							value={formData.password}
							onChange={handleChange}
							onFocus={() => handleFocus('password')}
							onBlur={handleBlur}
							style={getInputStyle('password')}
							placeholder="Create a password (min. 6 characters)"
							aria-invalid={!!errors.password}
							aria-describedby={errors.password ? 'password-error' : undefined}
							disabled={registerMutation.isPending}
						/>
						{errors.password && (
							<div id="password-error" style={styles.error} role="alert">
								{errors.password}
							</div>
						)}
					</div>

					{/* Confirm Password Field */}
					<div style={styles.inputGroup}>
						<label htmlFor="confirmPassword" style={styles.label}>
							Confirm Password
						</label>
						<input
							id="confirmPassword"
							name="confirmPassword"
							type="password"
							value={formData.confirmPassword}
							onChange={handleChange}
							onFocus={() => handleFocus('confirmPassword')}
							onBlur={handleBlur}
							style={getInputStyle('confirmPassword')}
							placeholder="Re-enter your password"
							aria-invalid={!!errors.confirmPassword}
							aria-describedby={
								errors.confirmPassword ? 'confirmPassword-error' : undefined
							}
							disabled={registerMutation.isPending}
						/>
						{errors.confirmPassword && (
							<div id="confirmPassword-error" style={styles.error} role="alert">
								{errors.confirmPassword}
							</div>
						)}
					</div>

					{/* Terms and Conditions Checkbox */}
					<div style={{ marginBottom: '24px' }}>
						<div style={styles.checkboxContainer}>
							<input
								id="acceptTerms"
								name="acceptTerms"
								type="checkbox"
								checked={formData.acceptTerms}
								onChange={handleChange}
								style={styles.checkbox}
								aria-invalid={!!errors.acceptTerms}
								aria-describedby={errors.acceptTerms ? 'terms-error' : undefined}
								disabled={registerMutation.isPending}
							/>
							<label htmlFor="acceptTerms" style={styles.checkboxLabel}>
								I accept the{' '}
								<a
									href="/terms"
									style={styles.link}
									target="_blank"
									rel="noopener noreferrer"
									onClick={e => e.stopPropagation()}
								>
									Terms and Conditions
								</a>
							</label>
						</div>
						{errors.acceptTerms && (
							<div id="terms-error" style={styles.error} role="alert">
								{errors.acceptTerms}
							</div>
						)}
					</div>

					{/* Submit Button */}
					<button
						type="submit"
						disabled={registerMutation.isPending}
						style={{
							...styles.button,
							...(registerMutation.isPending ? styles.buttonDisabled : {}),
						}}
						onMouseEnter={e => {
							if (!registerMutation.isPending) {
								(e.target as HTMLButtonElement).style.backgroundColor = '#3A7BC8';
							}
						}}
						onMouseLeave={e => {
							(e.target as HTMLButtonElement).style.backgroundColor = theme.primary;
						}}
						aria-busy={registerMutation.isPending}
					>
						{registerMutation.isPending ? 'Creating account...' : 'Sign Up'}
					</button>
				</form>

				{/* Link to Login */}
				<div style={styles.textCenter}>
					Already have an account?{' '}
					<Link
						to="/login"
						style={styles.link}
						onMouseEnter={e => {
							(e.target as HTMLAnchorElement).style.opacity = '0.8';
						}}
						onMouseLeave={e => {
							(e.target as HTMLAnchorElement).style.opacity = '1';
						}}
					>
						Sign in
					</Link>
				</div>
			</div>
		</div>
	);
};

export default Register;

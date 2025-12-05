import React, { useState, useEffect } from 'react';
import { getGroups } from '../api/groupController';
import { createClass } from '../api/classController';
import { ArrowLeft, Plus, Clock } from 'lucide-react';
import { useNavigate } from "react-router-dom";

// TypeScript Interfaces
interface Group {
  id: number;
  title: string;
  students: any[];
}

interface FormData {
  className: string;
  groupId: string;
  selectedDays: number[];
  startTime: string;
  duration: number;
  durationUnit: 'hours' | 'minutes';
}

interface ValidationErrors {
  className?: string;
  groupId?: string;
  selectedDays?: string;
  startTime?: string;
  duration?: string;
}



const AddClassScreen: React.FC = () => {
  // State Management
  const [formData, setFormData] = useState<FormData>({
    className: '',
    groupId: '',
    selectedDays: [],
    startTime: '',
    duration: 1,
    durationUnit: 'hours'
  });
  const [validationErrors, setValidationErrors] = useState<ValidationErrors>({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitSuccess, setSubmitSuccess] = useState(false);
  const [focusedField, setFocusedField] = useState<string>('');
  const [groups, setGroups] = useState<Group[]>([]);
  const [groupsLoading, setGroupsLoading] = useState(false);
  const [groupsError, setGroupsError] = useState<string | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);
  const navigate = useNavigate();

  // Fetch groups from API on mount
  useEffect(() => {
    const fetchGroups = async () => {
      setGroupsLoading(true);
      setGroupsError(null);
      try {
        const res = await getGroups();
        setGroups(res.data.items);
      } catch (err) {
        setGroupsError('Failed to load groups.');
      } finally {
        setGroupsLoading(false);
      }
    };
    fetchGroups();
  }, []);

  // Constants
  const DAYS = [
    { id: 0, name: 'Monday', short: 'Mon' },
    { id: 1, name: 'Tuesday', short: 'Tue' },
    { id: 2, name: 'Wednesday', short: 'Wed' },
    { id: 3, name: 'Thursday', short: 'Thu' },
    { id: 4, name: 'Friday', short: 'Fri' },
    { id: 5, name: 'Saturday', short: 'Sat' },
    { id: 6, name: 'Sunday', short: 'Sun' }
  ];

  // Theme colors (matching previous screens)
  const colors = {
    primary: '#6366F1',
    primaryHover: '#4F46E5',
    primaryLight: '#EEF2FF',
    background: '#F9FAFB',
    border: '#E5E7EB',
    borderFocus: '#C7D2FE',
    text: {
      primary: '#111827',
      secondary: '#6B7280',
      light: '#9CA3AF',
    },
    white: '#FFFFFF',
    success: '#10B981',
    successLight: '#D1FAE5',
    error: '#EF4444',
    errorLight: '#FEE2E2',
  };

  // Utility Functions
  const handleBackButton = () => {
    navigate(-1);
  };

  const validateForm = (): ValidationErrors => {
    const errors: ValidationErrors = {};

    if (!formData.className.trim()) {
      errors.className = 'Class name is required';
    }

    if (!formData.groupId) {
      errors.groupId = 'Please select a group';
    }

    if (formData.selectedDays.length === 0) {
      errors.selectedDays = 'Please select at least one day';
    }

    if (!formData.startTime) {
      errors.startTime = 'Start time is required';
    }

    if (!formData.duration || formData.duration <= 0) {
      errors.duration = 'Duration must be greater than 0';
    }

    return errors;
  };

  const handleInputChange = (field: keyof FormData, value: any) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));

    // Clear validation error for this field
    // Clear validation error for this field (only if it exists in ValidationErrors)
    if (field in validationErrors && validationErrors[field as keyof ValidationErrors]) {
    setValidationErrors(prev => ({
        ...prev,
        [field as keyof ValidationErrors]: undefined
    }));
    }
  };

  const toggleDay = (dayId: number) => {
    const newSelectedDays = formData.selectedDays.includes(dayId)
      ? formData.selectedDays.filter(id => id !== dayId)
      : [...formData.selectedDays, dayId];
    
    handleInputChange('selectedDays', newSelectedDays);
  };

  const handleSubmit = async () => {
    setIsSubmitting(true);
    setApiError(null);
    const errors = validateForm();
    if (Object.keys(errors).length > 0) {
      setValidationErrors(errors);
      setIsSubmitting(false);
      return;
    }

    try {
      // Prepare API payload
      const group = groups.find(g => g.id.toString() === formData.groupId);
      if (!group) throw new Error('Selected group not found');

      // Calculate start and end times (for demo, use today as base)
      const today = new Date();
      const startDate = today.toISOString().split('T')[0];
      const start = `${startDate}T00:00:00Z`;
      const end = `${startDate}T23:59:59Z`;

      // API expects: name, groupId, start, end, frequency, startTime, endTime
      await createClass({
        groupId: group.id,
        start,
        end
      });

      setSubmitSuccess(true);
      setTimeout(() => {
        setFormData({
          className: '',
          groupId: '',
          selectedDays: [],
          startTime: '',
          duration: 1,
          durationUnit: 'hours'
        });
        setSubmitSuccess(false);
        setValidationErrors({});
      }, 2000);
    } catch (error: any) {
      setApiError(error?.response?.data?.message || error.message || 'Error submitting form');
    } finally {
      setIsSubmitting(false);
    }
  };

  const formatDuration = (): string => {
    if (formData.durationUnit === 'hours') {
      return formData.duration === 1 ? '1 hour' : `${formData.duration} hours`;
    } else {
      return formData.duration === 1 ? '1 minute' : `${formData.duration} minutes`;
    }
  };

  return (
    <div style={{ minHeight: '100vh', backgroundColor: colors.background, padding: '2rem' }}>
      <div style={{ maxWidth: '800px', margin: '0 auto' }}>
        {/* Top Section - Title with Back Button */}
        <div style={{
          display: 'flex',
          alignItems: 'center',
          gap: '1rem',
          marginBottom: '2rem',
        }}>
          <button
            onClick={handleBackButton}
            style={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              padding: '0.75rem',
              borderRadius: '0.5rem',
              border: 'none',
              backgroundColor: colors.white,
              cursor: 'pointer',
              transition: 'all 0.2s',
              boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
            }}
            onMouseEnter={(e) => e.currentTarget.style.backgroundColor = colors.background}
            onMouseLeave={(e) => e.currentTarget.style.backgroundColor = colors.white}
            onMouseDown={(e) => e.currentTarget.style.transform = 'scale(0.95)'}
            onMouseUp={(e) => e.currentTarget.style.transform = 'scale(1)'}
          >
            <ArrowLeft size={20} color={colors.text.primary} />
          </button>
          
          <h1 style={{
            fontSize: '2rem',
            fontWeight: 'bold',
            color: colors.text.primary,
            margin: 0,
          }}>
            Add Class
          </h1>
        </div>

        {/* Success Message */}
        {submitSuccess && (
          <div style={{
            backgroundColor: colors.successLight,
            color: colors.success,
            padding: '1rem',
            borderRadius: '0.5rem',
            marginBottom: '2rem',
            fontSize: '0.875rem',
            fontWeight: '500',
            textAlign: 'center',
          }}>
            Class added successfully! Form will be cleared automatically.
          </div>
        )}

        {/* Form Card */}
        <div style={{
          backgroundColor: colors.white,
          borderRadius: '1rem',
          padding: '2rem',
          boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
        }}>
          {/* Class Name */}
          <div style={{ marginBottom: '2rem' }}>
            <label style={{
              display: 'block',
              fontSize: '0.875rem',
              fontWeight: '600',
              color: colors.text.primary,
              marginBottom: '0.5rem',
            }}>
              Class Name *
            </label>
            <input
              type="text"
              value={formData.className}
              onChange={(e) => handleInputChange('className', e.target.value)}
              onFocus={() => setFocusedField('className')}
              onBlur={() => setFocusedField('')}
              placeholder="Enter class name"
              style={{
                width: '100%',
                padding: '0.75rem',
                borderRadius: '0.5rem',
                border: `2px solid ${
                  validationErrors.className ? colors.error :
                  focusedField === 'className' ? colors.primary :
                  colors.border
                }`,
                fontSize: '0.875rem',
                color: colors.text.primary,
                backgroundColor: colors.white,
                outline: 'none',
                transition: 'all 0.2s',
                boxSizing: 'border-box',
              }}
            />
            {validationErrors.className && (
              <p style={{
                color: colors.error,
                fontSize: '0.75rem',
                marginTop: '0.25rem',
                margin: '0.25rem 0 0 0',
              }}>
                {validationErrors.className}
              </p>
            )}
          </div>

          {/* Group Selection */}
          <div style={{ marginBottom: '2rem' }}>
            <label style={{
              display: 'block',
              fontSize: '0.875rem',
              fontWeight: '600',
              color: colors.text.primary,
              marginBottom: '0.5rem',
            }}>
              Group *
            </label>
            {groupsLoading ? (
              <div style={{ color: colors.text.secondary, fontSize: '0.875rem', marginBottom: '0.5rem' }}>Loading groups...</div>
            ) : groupsError ? (
              <div style={{ color: colors.error, fontSize: '0.875rem', marginBottom: '0.5rem' }}>{groupsError}</div>
            ) : (
              <select
                value={formData.groupId}
                onChange={(e) => handleInputChange('groupId', e.target.value)}
                onFocus={() => setFocusedField('groupId')}
                onBlur={() => setFocusedField('')}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  borderRadius: '0.5rem',
                  border: `2px solid ${
                    validationErrors.groupId ? colors.error :
                    focusedField === 'groupId' ? colors.primary :
                    colors.border
                  }`,
                  fontSize: '0.875rem',
                  color: formData.groupId ? colors.text.primary : colors.text.secondary,
                  backgroundColor: colors.white,
                  outline: 'none',
                  transition: 'all 0.2s',
                  cursor: 'pointer',
                  boxSizing: 'border-box',
                }}
              >
                <option value="">Select a group</option>
                {groups.map(group => (
                  <option key={group.id} value={group.id}>
                    {group.title}
                  </option>
                ))}
              </select>
            )}
            {validationErrors.groupId && (
              <p style={{
                color: colors.error,
                fontSize: '0.75rem',
                marginTop: '0.25rem',
                margin: '0.25rem 0 0 0',
              }}>
                {validationErrors.groupId}
              </p>
            )}
          </div>

          {/* Days of Week */}
          <div style={{ marginBottom: '2rem' }}>
            <label style={{
              display: 'block',
              fontSize: '0.875rem',
              fontWeight: '600',
              color: colors.text.primary,
              marginBottom: '0.75rem',
            }}>
              Days of Week *
            </label>
            <div style={{
              display: 'flex',
              flexWrap: 'wrap',
              gap: '0.5rem',
            }}>
              {DAYS.map(day => {
                const isSelected = formData.selectedDays.includes(day.id);
                return (
                  <button
                    key={day.id}
                    type="button"
                    onClick={() => toggleDay(day.id)}
                    style={{
                      padding: '0.5rem 1rem',
                      borderRadius: '2rem',
                      border: `2px solid ${isSelected ? colors.primary : colors.border}`,
                      backgroundColor: isSelected ? colors.primary : colors.white,
                      color: isSelected ? colors.white : colors.text.primary,
                      fontSize: '0.875rem',
                      fontWeight: '500',
                      cursor: 'pointer',
                      transition: 'all 0.2s',
                      outline: 'none',
                    }}
                    onMouseEnter={(e) => {
                      if (!isSelected) {
                        e.currentTarget.style.backgroundColor = colors.primaryLight;
                        e.currentTarget.style.borderColor = colors.primary;
                      }
                    }}
                    onMouseLeave={(e) => {
                      if (!isSelected) {
                        e.currentTarget.style.backgroundColor = colors.white;
                        e.currentTarget.style.borderColor = colors.border;
                      }
                    }}
                  >
                    {day.short}
                  </button>
                );
              })}
            </div>
            {validationErrors.selectedDays && (
              <p style={{
                color: colors.error,
                fontSize: '0.75rem',
                marginTop: '0.5rem',
                margin: '0.5rem 0 0 0',
              }}>
                {validationErrors.selectedDays}
              </p>
            )}
          </div>

          {/* Start Time and Duration Row */}
          <div style={{
            display: 'flex',
            gap: '1.5rem',
            marginBottom: '2rem',
            flexWrap: 'wrap',
          }}>
            {/* Start Time */}
            <div style={{ flex: '1', minWidth: '200px' }}>
              <label style={{
                display: 'block',
                fontSize: '0.875rem',
                fontWeight: '600',
                color: colors.text.primary,
                marginBottom: '0.5rem',
              }}>
                Start Time *
              </label>
              <div style={{ position: 'relative' }}>
                <input
                  type="time"
                  value={formData.startTime}
                  onChange={(e) => handleInputChange('startTime', e.target.value)}
                  onFocus={() => setFocusedField('startTime')}
                  onBlur={() => setFocusedField('')}
                  style={{
                    width: '100%',
                    padding: '0.75rem',
                    paddingLeft: '2.5rem',
                    borderRadius: '0.5rem',
                    border: `2px solid ${
                      validationErrors.startTime ? colors.error :
                      focusedField === 'startTime' ? colors.primary :
                      colors.border
                    }`,
                    fontSize: '0.875rem',
                    color: colors.text.primary,
                    backgroundColor: colors.white,
                    outline: 'none',
                    transition: 'all 0.2s',
                    boxSizing: 'border-box',
                  }}
                />
                <Clock
                  size={18}
                  style={{
                    position: 'absolute',
                    left: '0.75rem',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    color: colors.text.secondary,
                  }}
                />
              </div>
              {validationErrors.startTime && (
                <p style={{
                  color: colors.error,
                  fontSize: '0.75rem',
                  marginTop: '0.25rem',
                  margin: '0.25rem 0 0 0',
                }}>
                  {validationErrors.startTime}
                </p>
              )}
            </div>

            {/* Duration */}
            <div style={{ flex: '1', minWidth: '200px' }}>
              <label style={{
                display: 'block',
                fontSize: '0.875rem',
                fontWeight: '600',
                color: colors.text.primary,
                marginBottom: '0.5rem',
              }}>
                Duration *
              </label>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <input
                  type="number"
                  min="1"
                  max="24"
                  value={formData.duration}
                  onChange={(e) => handleInputChange('duration', parseFloat(e.target.value) || 0)}
                  onFocus={() => setFocusedField('duration')}
                  onBlur={() => setFocusedField('')}
                  style={{
                    flex: '2',
                    padding: '0.75rem',
                    borderRadius: '0.5rem',
                    border: `2px solid ${
                      validationErrors.duration ? colors.error :
                      focusedField === 'duration' ? colors.primary :
                      colors.border
                    }`,
                    fontSize: '0.875rem',
                    color: colors.text.primary,
                    backgroundColor: colors.white,
                    outline: 'none',
                    transition: 'all 0.2s',
                    boxSizing: 'border-box',
                  }}
                />
                <select
                  value={formData.durationUnit}
                  onChange={(e) => handleInputChange('durationUnit', e.target.value as 'hours' | 'minutes')}
                  style={{
                    flex: '1',
                    padding: '0.75rem',
                    borderRadius: '0.5rem',
                    border: `2px solid ${colors.border}`,
                    fontSize: '0.875rem',
                    color: colors.text.primary,
                    backgroundColor: colors.white,
                    outline: 'none',
                    cursor: 'pointer',
                    boxSizing: 'border-box',
                  }}
                >
                  <option value="hours">Hours</option>
                  <option value="minutes">Minutes</option>
                </select>
              </div>
              {validationErrors.duration && (
                <p style={{
                  color: colors.error,
                  fontSize: '0.75rem',
                  marginTop: '0.25rem',
                  margin: '0.25rem 0 0 0',
                }}>
                  {validationErrors.duration}
                </p>
              )}
            </div>
          </div>

          {/* Form Summary */}
          {formData.className && formData.groupId && formData.selectedDays.length > 0 && formData.startTime && formData.duration > 0 && (
            <div style={{
              backgroundColor: colors.primaryLight,
              padding: '1rem',
              borderRadius: '0.5rem',
              marginBottom: '2rem',
              border: `1px solid ${colors.borderFocus}`,
            }}>
              <h3 style={{
                fontSize: '0.875rem',
                fontWeight: '600',
                color: colors.text.primary,
                marginBottom: '0.5rem',
                margin: '0 0 0.5rem 0',
              }}>
                Class Summary:
              </h3>
              <div style={{
                fontSize: '0.875rem',
                color: colors.text.secondary,
                lineHeight: '1.5',
              }}>
                <p style={{ margin: '0 0 0.25rem 0' }}>
                  <strong>{formData.className}</strong> for {groups.find(g => g.id.toString() === formData.groupId)?.title}
                </p>
                        {/* API Error */}
                        {apiError && (
                          <div style={{
                            backgroundColor: colors.errorLight,
                            color: colors.error,
                            padding: '1rem',
                            borderRadius: '0.5rem',
                            marginBottom: '2rem',
                            fontSize: '0.875rem',
                            fontWeight: '500',
                            textAlign: 'center',
                          }}>
                            {apiError}
                          </div>
                        )}
                <p style={{ margin: '0 0 0.25rem 0' }}>
                  {DAYS.filter(day => formData.selectedDays.includes(day.id)).map(day => day.name).join(', ')}
                </p>
                <p style={{ margin: '0' }}>
                  {formData.startTime} for {formatDuration()}
                </p>
              </div>
            </div>
          )}

          {/* Submit Button */}
          <button
            onClick={handleSubmit}
            disabled={isSubmitting || submitSuccess}
            style={{
              width: '100%',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              gap: '0.5rem',
              padding: '1rem',
              borderRadius: '0.5rem',
              border: 'none',
              backgroundColor: submitSuccess ? colors.success : colors.primary,
              color: colors.white,
              fontSize: '1rem',
              fontWeight: '600',
              cursor: isSubmitting || submitSuccess ? 'not-allowed' : 'pointer',
              transition: 'all 0.2s',
              opacity: isSubmitting ? 0.7 : 1,
            }}
            onMouseEnter={(e) => {
              if (!isSubmitting && !submitSuccess) {
                e.currentTarget.style.backgroundColor = colors.primaryHover;
              }
            }}
            onMouseLeave={(e) => {
              if (!isSubmitting && !submitSuccess) {
                e.currentTarget.style.backgroundColor = submitSuccess ? colors.success : colors.primary;
              }
            }}
            onMouseDown={(e) => {
              if (!isSubmitting && !submitSuccess) {
                e.currentTarget.style.transform = 'scale(0.98)';
              }
            }}
            onMouseUp={(e) => {
              if (!isSubmitting && !submitSuccess) {
                e.currentTarget.style.transform = 'scale(1)';
              }
            }}
          >
            {isSubmitting ? (
              <>
                <div style={{
                  width: '20px',
                  height: '20px',
                  border: '2px solid transparent',
                  borderTop: '2px solid white',
                  borderRadius: '50%',
                  animation: 'spin 1s linear infinite',
                }} />
                Adding Class...
              </>
            ) : submitSuccess ? (
              <>
                ✓ Class Added Successfully!
              </>
            ) : (
              <>
                <Plus size={20} />
                Add Class
              </>
            )}
          </button>
        </div>
      </div>

      <style>{`
        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }
      `}</style>
    </div>
  );
};

export default AddClassScreen;

import { updateAttendance } from '../api/attendanceController';
import React, { useState, useMemo, useEffect } from 'react';
import { ArrowLeft, ChevronLeft, ChevronRight, Download } from 'lucide-react';
import { useNavigate, useLocation } from "react-router-dom";
import { getGroups } from '../api/groupController';

// TypeScript Interfaces
interface Group {
  id: number;
  name: string;
  color: string;
}

interface Student {
  id: number;
  name: string;
  groupId: number;
}

interface AttendanceRecord {
  studentId: number;
  date: string; // ISO date string
  present: boolean;
}

// Remove mockGroups, use API

import { getStudentsByGroup } from '../api/studentController';

const ClassAttendanceScreen: React.FC = () => {
    // Confirm attendance state
    const [confirmLoading, setConfirmLoading] = useState(false);
    const [confirmError, setConfirmError] = useState<string | null>(null);
    const [confirmSuccess, setConfirmSuccess] = useState<string | null>(null);

    // Handler to send attendance to backend
    const handleConfirmAttendance = async () => {
      if (!state?.id) {
        setConfirmError('Class ID not found.');
        return;
      }
      setConfirmLoading(true);
      setConfirmError(null);
      setConfirmSuccess(null);
      try {
        // Only send present student IDs for the week
        const presentStudentIds = attendanceRecords
          .filter((r: AttendanceRecord) => r.present)
          .map((r: AttendanceRecord) => r.studentId);
        await updateAttendance(state.id, presentStudentIds);
        setConfirmSuccess('Attendance submitted successfully!');
      } catch (e) {
        setConfirmError('Failed to submit attendance.');
      } finally {
        setConfirmLoading(false);
      }
    };
  // State Management
  const [currentWeekStart, setCurrentWeekStart] = useState<Date>(() => {
    const today = new Date();
    const dayOfWeek = today.getDay();
    const diff = dayOfWeek === 0 ? -6 : 1 - dayOfWeek;
    const monday = new Date(today);
    monday.setDate(today.getDate() + diff);
    monday.setHours(0, 0, 0, 0);
    return monday;
  });  
  const [groups, setGroups] = useState<Group[]>([]);
  const [groupsLoading, setGroupsLoading] = useState<boolean>(true);
  const [groupsError, setGroupsError] = useState<string | null>(null);
  const [selectedGroupId, setSelectedGroupId] = useState<number | null>(null);
    // Fetch groups from API
    useEffect(() => {
      setGroupsLoading(true);
      getGroups()
        .then(res => {
          setGroups(res.data.items || res.data);
          setGroupsError(null);
          if ((res.data.items && res.data.items.length > 0) || (res.data.length > 0)) {
            setSelectedGroupId((res.data.items?.[0]?.id ?? res.data[0]?.id) ?? null);
          }
        })
        .catch(() => setGroupsError('Failed to load groups'))
        .finally(() => setGroupsLoading(false));
    }, []);
  const [attendanceRecords, setAttendanceRecords] = useState<AttendanceRecord[]>([]);
  const navigate = useNavigate();
  const location = useLocation();
  const { state } = location;

  // Theme colors (matching calendar screen)
  const colors = {
    primary: '#6366F1',
    primaryHover: '#4F46E5',
    background: '#F9FAFB',
    border: '#E5E7EB',
    text: {
      primary: '#111827',
      secondary: '#6B7280',
      light: '#9CA3AF',
    },
    white: '#FFFFFF',
    success: '#10B981',
    currentDay: '#EEF2FF',
  };

  // Utility Functions
  const getWeekDates = (startDate: Date): Date[] => {
    return Array.from({ length: 7 }, (_, i) => {
      const date = new Date(startDate);
      date.setDate(startDate.getDate() + i);
      return date;
    });
  };

  const formatDateShort = (date: Date): string => {
    const days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    const dayName = days[date.getDay() === 0 ? 6 : date.getDay() - 1];
    const dayNum = date.getDate();
    return `${dayName} ${dayNum}`;
  };

  const formatDateISO = (date: Date): string => {
    return date.toISOString().split('T')[0];
  };

  const isToday = (date: Date): boolean => {
    const today = new Date();
    return date.toDateString() === today.toDateString();
  };

  const navigateWeek = (direction: 'prev' | 'next') => {
    const newDate = new Date(currentWeekStart);
    newDate.setDate(currentWeekStart.getDate() + (direction === 'next' ? 7 : -7));
    setCurrentWeekStart(newDate);
  };

  const handleBackButton = () => {
    console.log('Navigate back to calendar screen');
    navigate(-1);
  };

  // Students state
  const [students, setStudents] = useState<Student[]>([]);
  const [studentsLoading, setStudentsLoading] = useState<boolean>(false);
  const [studentsError, setStudentsError] = useState<string | null>(null);

  // Fetch students when group changes
  useEffect(() => {
    if (!selectedGroupId) {
      setStudents([]);
      return;
    }
    setStudentsLoading(true);
    getStudentsByGroup(selectedGroupId)
      .then(res => {
        setStudents(res.data.items || res.data);
        setStudentsError(null);
      })
      .catch(() => setStudentsError('Failed to load students'))
      .finally(() => setStudentsLoading(false));
  }, [selectedGroupId]);

  const filteredStudents = students;

  // Get week dates
  const weekDates = useMemo(() => getWeekDates(currentWeekStart), [currentWeekStart]);

  // Check if student is present on a date
  const isPresent = (studentId: number, date: Date): boolean => {
    const dateStr = formatDateISO(date);
    const record = attendanceRecords.find(
      r => r.studentId === studentId && r.date === dateStr
    );
    return record?.present || false;
  };

  // Toggle individual attendance
  const toggleAttendance = (studentId: number, date: Date) => {
    const dateStr = formatDateISO(date);
    setAttendanceRecords(prev => {
      const existingIndex = prev.findIndex(
        r => r.studentId === studentId && r.date === dateStr
      );
      
      if (existingIndex >= 0) {
        const updated = [...prev];
        updated[existingIndex] = {
          ...updated[existingIndex],
          present: !updated[existingIndex].present
        };
        return updated;
      } else {
        return [...prev, { studentId, date: dateStr, present: true }];
      }
    });
  };

  // Toggle all students for a specific day
  const toggleDayAttendance = (date: Date) => {
    const dateStr = formatDateISO(date);
    const allPresent = filteredStudents.every(student => isPresent(student.id, date));
    
    setAttendanceRecords(prev => {
      let updated = [...prev];
      
      filteredStudents.forEach(student => {
        const existingIndex = updated.findIndex(
          r => r.studentId === student.id && r.date === dateStr
        );
        
        if (existingIndex >= 0) {
          updated[existingIndex] = {
            ...updated[existingIndex],
            present: !allPresent
          };
        } else {
          updated.push({
            studentId: student.id,
            date: dateStr,
            present: true
          });
        }
      });
      
      return updated;
    });
  };

  // Export CSV
  const exportCSV = () => {
    const today = new Date();
    const firstDayOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayOfMonth = new Date(today.getFullYear(), today.getMonth() + 1, 0);
    
    // Generate all dates in the month
    const monthDates: Date[] = [];
    for (let d = new Date(firstDayOfMonth); d <= lastDayOfMonth; d.setDate(d.getDate() + 1)) {
      monthDates.push(new Date(d));
    }
    
    // Build CSV content
    let csvContent = 'Student Name';
    monthDates.forEach(date => {
      csvContent += `,${formatDateShort(date)}`;
    });
    csvContent += '\n';
    
    filteredStudents.forEach(student => {
      csvContent += student.name;
      monthDates.forEach(date => {
        const present = isPresent(student.id, date);
        csvContent += `,${present ? 'Present' : 'Absent'}`;
      });
      csvContent += '\n';
    });
    
    // Download CSV
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', `attendance_${state.name}_${today.getMonth() + 1}_${today.getFullYear()}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <div style={{ minHeight: '100vh', backgroundColor: colors.background, padding: '2rem' }}>
      <div style={{ maxWidth: '1400px', margin: '0 auto' }}>
        {/* Top Section - Class Name with Back Button */}
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
            {state.name}
          </h1>
        </div>

        {/* Control Row */}
        <div style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          marginBottom: '2rem',
          gap: '1rem',
          flexWrap: 'wrap',
          backgroundColor: colors.white,
          padding: '1.5rem',
          borderRadius: '1rem',
          boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)'
        }}>
          {/* Left - Group Selection */}
          <div>
            {groupsLoading ? (
              <span>Loading groups...</span>
            ) : groupsError ? (
              <span style={{ color: 'red' }}>{groupsError}</span>
            ) : (
              <select
                value={selectedGroupId ?? ''}
                onChange={(e) => setSelectedGroupId(Number(e.target.value))}
                style={{
                  padding: '0.625rem 1rem',
                  borderRadius: '0.5rem',
                  border: `1px solid ${colors.border}`,
                  fontSize: '0.875rem',
                  color: colors.text.primary,
                  backgroundColor: colors.white,
                  cursor: 'pointer',
                  outline: 'none',
                  transition: 'all 0.2s',
                  minWidth: '150px',
                }}
                onMouseEnter={(e) => e.currentTarget.style.borderColor = colors.primary}
                onMouseLeave={(e) => e.currentTarget.style.borderColor = colors.border}
              >
                {groups.map(group => (
                  <option key={group.id} value={group.id}>{group.name}</option>
                ))}
              </select>
            )}
          </div>

          {/* Right - Export CSV */}
          <button
            onClick={exportCSV}
            style={{
              display: 'flex',
              alignItems: 'center',
              gap: '0.5rem',
              padding: '0.625rem 1.25rem',
              borderRadius: '0.5rem',
              border: `1px solid ${colors.border}`,
              backgroundColor: colors.white,
              color: colors.text.primary,
              fontSize: '0.875rem',
              fontWeight: '500',
              cursor: 'pointer',
              transition: 'all 0.2s',
            }}
            onMouseEnter={(e) => {
              e.currentTarget.style.backgroundColor = colors.background;
              e.currentTarget.style.borderColor = colors.primary;
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.backgroundColor = colors.white;
              e.currentTarget.style.borderColor = colors.border;
            }}
            onMouseDown={(e) => e.currentTarget.style.transform = 'scale(0.95)'}
            onMouseUp={(e) => e.currentTarget.style.transform = 'scale(1)'}
          >
            <Download size={18} />
            Export CSV
          </button>
        </div>

        {/* Confirm Attendance Button */}
        <div style={{ marginBottom: '1rem', display: 'flex', gap: '1rem', alignItems: 'center' }}>
          <button
            onClick={handleConfirmAttendance}
            disabled={confirmLoading || studentsLoading || !filteredStudents.length}
            style={{
              padding: '0.75rem 1.5rem',
              borderRadius: '0.5rem',
              border: 'none',
              backgroundColor: confirmLoading ? colors.text.light : colors.primary,
              color: colors.white,
              fontWeight: 600,
              fontSize: '1rem',
              cursor: confirmLoading ? 'not-allowed' : 'pointer',
              transition: 'all 0.2s',
            }}
          >
            {confirmLoading ? 'Submitting...' : 'Confirm Attendance'}
          </button>
          {confirmError && <span style={{ color: 'red' }}>{confirmError}</span>}
          {confirmSuccess && <span style={{ color: 'green' }}>{confirmSuccess}</span>}
        </div>
        {/* Attendance Grid */}
        <div style={{
          backgroundColor: colors.white,
          borderRadius: '1rem',
          padding: '1.5rem',
          boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
          overflowX: 'auto'
        }}>
          {/* Week Navigation */}
          <div style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            gap: '1rem',
            marginBottom: '1.5rem',
          }}>
            <button
              onClick={() => navigateWeek('prev')}
              style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                padding: '0.5rem',
                borderRadius: '0.5rem',
                border: `1px solid ${colors.border}`,
                backgroundColor: colors.white,
                cursor: 'pointer',
                transition: 'all 0.2s',
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.backgroundColor = colors.background;
                e.currentTarget.style.borderColor = colors.primary;
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.backgroundColor = colors.white;
                e.currentTarget.style.borderColor = colors.border;
              }}
            >
              <ChevronLeft size={20} color={colors.text.primary} />
            </button>
            
            <span style={{
              fontSize: '1rem',
              fontWeight: '600',
              color: colors.text.primary,
            }}>
              Week View
            </span>
            
            <button
              onClick={() => navigateWeek('next')}
              style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                padding: '0.5rem',
                borderRadius: '0.5rem',
                border: `1px solid ${colors.border}`,
                backgroundColor: colors.white,
                cursor: 'pointer',
                transition: 'all 0.2s',
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.backgroundColor = colors.background;
                e.currentTarget.style.borderColor = colors.primary;
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.backgroundColor = colors.white;
                e.currentTarget.style.borderColor = colors.border;
              }}
            >
              <ChevronRight size={20} color={colors.text.primary} />
            </button>
          </div>

          {/* Table */}
          <div style={{ minWidth: '800px' }}>
            <table style={{
              width: '100%',
              borderCollapse: 'separate',
              borderSpacing: 0,
            }}>
              <thead>
                <tr>
                  {/* Student Name Column Header */}
                  <th style={{
                    padding: '1rem',
                    textAlign: 'left',
                    fontSize: '0.875rem',
                    fontWeight: '600',
                    color: colors.text.primary,
                    backgroundColor: colors.background,
                    borderTopLeftRadius: '0.5rem',
                    border: `1px solid ${colors.border}`,
                    borderRight: 'none',
                  }}>
                    Student Name
                  </th>
                  
                  {/* Day Column Headers */}
                  {weekDates.map((date, index) => {
                    const isTodayDate = isToday(date);
                    return (
                      <th
                        key={index}
                        onClick={() => toggleDayAttendance(date)}
                        style={{
                          padding: '1rem',
                          textAlign: 'center',
                          fontSize: '0.875rem',
                          fontWeight: '600',
                          color: isTodayDate ? colors.primary : colors.text.primary,
                          backgroundColor: isTodayDate ? colors.currentDay : colors.background,
                          border: `1px solid ${colors.border}`,
                          borderLeft: 'none',
                          borderRight: index === weekDates.length - 1 ? `1px solid ${colors.border}` : 'none',
                          borderTopRightRadius: index === weekDates.length - 1 ? '0.5rem' : 0,
                          cursor: 'pointer',
                          transition: 'all 0.2s',
                        }}
                        onMouseEnter={(e) => {
                          if (!isTodayDate) {
                            e.currentTarget.style.backgroundColor = colors.currentDay;
                          }
                        }}
                        onMouseLeave={(e) => {
                          if (!isTodayDate) {
                            e.currentTarget.style.backgroundColor = colors.background;
                          }
                        }}
                      >
                        {formatDateShort(date)}
                      </th>
                    );
                  })}
                </tr>
              </thead>
              
              <tbody>
                {studentsLoading ? (
                  <tr><td colSpan={weekDates.length + 1}>Loading students...</td></tr>
                ) : studentsError ? (
                  <tr><td colSpan={weekDates.length + 1} style={{ color: 'red' }}>{studentsError}</td></tr>
                ) : filteredStudents.length === 0 ? (
                  <tr><td colSpan={weekDates.length + 1}>No students found for this group.</td></tr>
                ) : filteredStudents.map((student, studentIndex) => (
                  <tr key={student.id}>
                    {/* Student Name Cell */}
                    <td style={{
                      padding: '1rem',
                      fontSize: '0.875rem',
                      color: colors.text.primary,
                      backgroundColor: colors.white,
                      border: `1px solid ${colors.border}`,
                      borderTop: 'none',
                      borderRight: 'none',
                      borderBottomLeftRadius: studentIndex === filteredStudents.length - 1 ? '0.5rem' : 0,
                    }}>
                      {student.name}
                    </td>
                    
                    {/* Attendance Checkboxes */}
                    {weekDates.map((date, dateIndex) => {
                      const present = isPresent(student.id, date);
                      const isTodayDate = isToday(date);
                      
                      return (
                        <td
                          key={dateIndex}
                          style={{
                            padding: '1rem',
                            textAlign: 'center',
                            backgroundColor: isTodayDate ? colors.currentDay : colors.white,
                            border: `1px solid ${colors.border}`,
                            borderTop: 'none',
                            borderLeft: 'none',
                            borderRight: dateIndex === weekDates.length - 1 ? `1px solid ${colors.border}` : 'none',
                            borderBottomRightRadius: studentIndex === filteredStudents.length - 1 && dateIndex === weekDates.length - 1 ? '0.5rem' : 0,
                          }}
                        >
                          <input
                            type="checkbox"
                            checked={present}
                            onChange={() => toggleAttendance(student.id, date)}
                            style={{
                              width: '18px',
                              height: '18px',
                              cursor: 'pointer',
                              accentColor: colors.primary,
                            }}
                          />
                        </td>
                      );
                    })}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* Info Footer */}
          <div style={{
            marginTop: '1.5rem',
            padding: '1rem',
            backgroundColor: colors.background,
            borderRadius: '0.5rem',
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
            flexWrap: 'wrap',
            gap: '1rem',
          }}>
            <div style={{
              fontSize: '0.875rem',
              color: colors.text.secondary,
            }}>
              <strong>{filteredStudents.length}</strong> students in {groups.find(g => g.id === selectedGroupId)?.name}
            </div>
            <div style={{
              fontSize: '0.75rem',
              color: colors.text.light,
            }}>
              Click day headers to toggle entire day • Click checkboxes for individual attendance
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ClassAttendanceScreen;

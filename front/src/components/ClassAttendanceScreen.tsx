/* eslint-disable sonarjs/no-duplicate-string */
import { ArrowLeft, ChevronLeft, ChevronRight, Download } from "lucide-react";
import React, { useState, useMemo } from "react";
import { useNavigate, useLocation } from "react-router-dom";

// TypeScript Interfaces
interface Group {
  id: string;
  name: string;
  color: string;
}

interface Student {
  id: string;
  name: string;
  groupId: string;
}

interface AttendanceRecord {
  studentId: string;
  date: string; // ISO date string
  present: boolean;
}

// Mock Data
const mockGroups: Group[] = [
  { id: "1", name: "Group 1", color: "#10B981" },
  { id: "2", name: "Group 2", color: "#3B82F6" },
  { id: "3", name: "Group 3", color: "#F59E0B" },
  { id: "4", name: "Group 4", color: "#EF4444" },
  { id: "5", name: "Group 5", color: "#8B5CF6" },
];

const mockStudents: Student[] = [
  { id: "1", name: "Alice Johnson", groupId: "1" },
  { id: "2", name: "Bob Smith", groupId: "1" },
  { id: "3", name: "Charlie Brown", groupId: "1" },
  { id: "4", name: "Diana Prince", groupId: "1" },
  { id: "5", name: "Ethan Hunt", groupId: "1" },
  { id: "6", name: "Fiona Apple", groupId: "2" },
  { id: "7", name: "George Wilson", groupId: "2" },
  { id: "8", name: "Hannah Montana", groupId: "2" },
  { id: "9", name: "Ian Malcolm", groupId: "3" },
  { id: "10", name: "Julia Roberts", groupId: "3" },
  { id: "11", name: "Kevin Hart", groupId: "4" },
  { id: "12", name: "Laura Croft", groupId: "4" },
  { id: "13", name: "Michael Scott", groupId: "5" },
  { id: "14", name: "Nina Simone", groupId: "5" },
];

const ClassAttendanceScreen: React.FC = () => {
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
  const [selectedGroupId, setSelectedGroupId] = useState<string>("1");
  const [attendanceRecords, setAttendanceRecords] = useState<
    AttendanceRecord[]
  >([]);
  const navigate = useNavigate();
  const location = useLocation();
  const { state } = location;

  // Theme colors (matching calendar screen)
  const colors = {
    primary: "#6366F1",
    primaryHover: "#4F46E5",
    background: "#F9FAFB",
    border: "#E5E7EB",
    text: {
      primary: "#111827",
      secondary: "#6B7280",
      light: "#9CA3AF",
    },
    white: "#FFFFFF",
    success: "#10B981",
    currentDay: "#EEF2FF",
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
    const days = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];
    const dayName = days[date.getDay() === 0 ? 6 : date.getDay() - 1];
    const dayNum = date.getDate();
    return `${dayName} ${dayNum}`;
  };

  const formatDateISO = (date: Date): string => {
    return date.toISOString().split("T")[0];
  };

  const isToday = (date: Date): boolean => {
    const today = new Date();
    return date.toDateString() === today.toDateString();
  };

  const navigateWeek = (direction: "prev" | "next") => {
    const newDate = new Date(currentWeekStart);
    newDate.setDate(
      currentWeekStart.getDate() + (direction === "next" ? 7 : -7),
    );
    setCurrentWeekStart(newDate);
  };

  const handleBackButton = () => {
    navigate(-1);
  };

  // Get filtered students
  const filteredStudents = useMemo(() => {
    return mockStudents.filter(
      (student) => student.groupId === selectedGroupId,
    );
  }, [selectedGroupId]);

  // Get week dates
  const weekDates = useMemo(
    () => getWeekDates(currentWeekStart),
    [currentWeekStart],
  );

  // Check if student is present on a date
  const isPresent = (studentId: string, date: Date): boolean => {
    const dateStr = formatDateISO(date);
    const record = attendanceRecords.find(
      (r) => r.studentId === studentId && r.date === dateStr,
    );
    return record?.present || false;
  };

  // Toggle individual attendance
  const toggleAttendance = (studentId: string, date: Date) => {
    const dateStr = formatDateISO(date);
    setAttendanceRecords((prev) => {
      const existingIndex = prev.findIndex(
        (r) => r.studentId === studentId && r.date === dateStr,
      );

      if (existingIndex >= 0) {
        const updated = [...prev];
        updated[existingIndex] = {
          ...updated[existingIndex],
          present: !updated[existingIndex].present,
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
    const allPresent = filteredStudents.every((student) =>
      isPresent(student.id, date),
    );

    setAttendanceRecords((prev) => {
      const updated = [...prev];

      filteredStudents.forEach((student) => {
        const existingIndex = updated.findIndex(
          (r) => r.studentId === student.id && r.date === dateStr,
        );

        if (existingIndex >= 0) {
          updated[existingIndex] = {
            ...updated[existingIndex],
            present: !allPresent,
          };
        } else {
          updated.push({
            studentId: student.id,
            date: dateStr,
            present: true,
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
    const lastDayOfMonth = new Date(
      today.getFullYear(),
      today.getMonth() + 1,
      0,
    );

    // Generate all dates in the month
    const monthDates: Date[] = [];
    for (
      let d = new Date(firstDayOfMonth);
      d <= lastDayOfMonth;
      d.setDate(d.getDate() + 1)
    ) {
      monthDates.push(new Date(d));
    }

    // Build CSV content
    let csvContent = "Student Name";
    monthDates.forEach((date) => {
      csvContent += `,${formatDateShort(date)}`;
    });
    csvContent += "\n";

    filteredStudents.forEach((student) => {
      csvContent += student.name;
      monthDates.forEach((date) => {
        const present = isPresent(student.id, date);
        csvContent += `,${present ? "Present" : "Absent"}`;
      });
      csvContent += "\n";
    });

    // Download CSV
    const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
    const link = document.createElement("a");
    const url = URL.createObjectURL(blob);
    link.setAttribute("href", url);
    link.setAttribute(
      "download",
      `attendance_${state.name}_${today.getMonth() + 1}_${today.getFullYear()}.csv`,
    );
    link.style.visibility = "hidden";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        backgroundColor: colors.background,
        padding: "2rem",
      }}
    >
      <div style={{ maxWidth: "1400px", margin: "0 auto" }}>
        {/* Top Section - Class Name with Back Button */}
        <div
          style={{
            display: "flex",
            alignItems: "center",
            gap: "1rem",
            marginBottom: "2rem",
          }}
        >
          <button
            onClick={handleBackButton}
            style={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              padding: "0.75rem",
              borderRadius: "0.5rem",
              border: "none",
              backgroundColor: colors.white,
              cursor: "pointer",
              transition: "all 0.2s",
              boxShadow: "0 1px 3px rgba(0, 0, 0, 0.1)",
            }}
            onMouseEnter={(e) =>
              (e.currentTarget.style.backgroundColor = colors.background)
            }
            onMouseLeave={(e) =>
              (e.currentTarget.style.backgroundColor = colors.white)
            }
            onMouseDown={(e) =>
              (e.currentTarget.style.transform = "scale(0.95)")
            }
            onMouseUp={(e) => (e.currentTarget.style.transform = "scale(1)")}
          >
            <ArrowLeft size={20} color={colors.text.primary} />
          </button>

          <h1
            style={{
              fontSize: "2rem",
              fontWeight: "bold",
              color: colors.text.primary,
              margin: 0,
            }}
          >
            {state.name}
          </h1>
        </div>

        {/* Control Row */}
        <div
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            marginBottom: "2rem",
            gap: "1rem",
            flexWrap: "wrap",
            backgroundColor: colors.white,
            padding: "1.5rem",
            borderRadius: "1rem",
            boxShadow: "0 1px 3px rgba(0, 0, 0, 0.1)",
          }}
        >
          {/* Left - Group Selection */}
          <div>
            <select
              value={selectedGroupId}
              onChange={(e) => setSelectedGroupId(e.target.value)}
              style={{
                padding: "0.625rem 1rem",
                borderRadius: "0.5rem",
                border: `1px solid ${colors.border}`,
                fontSize: "0.875rem",
                color: colors.text.primary,
                backgroundColor: colors.white,
                cursor: "pointer",
                outline: "none",
                transition: "all 0.2s",
                minWidth: "150px",
              }}
              onMouseEnter={(e) =>
                (e.currentTarget.style.borderColor = colors.primary)
              }
              onMouseLeave={(e) =>
                (e.currentTarget.style.borderColor = colors.border)
              }
            >
              {mockGroups.map((group) => (
                <option key={group.id} value={group.id}>
                  {group.name}
                </option>
              ))}
            </select>
          </div>

          {/* Right - Export CSV */}
          <button
            onClick={exportCSV}
            style={{
              display: "flex",
              alignItems: "center",
              gap: "0.5rem",
              padding: "0.625rem 1.25rem",
              borderRadius: "0.5rem",
              border: `1px solid ${colors.border}`,
              backgroundColor: colors.white,
              color: colors.text.primary,
              fontSize: "0.875rem",
              fontWeight: "500",
              cursor: "pointer",
              transition: "all 0.2s",
            }}
            onMouseEnter={(e) => {
              e.currentTarget.style.backgroundColor = colors.background;
              e.currentTarget.style.borderColor = colors.primary;
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.backgroundColor = colors.white;
              e.currentTarget.style.borderColor = colors.border;
            }}
            onMouseDown={(e) =>
              (e.currentTarget.style.transform = "scale(0.95)")
            }
            onMouseUp={(e) => (e.currentTarget.style.transform = "scale(1)")}
          >
            <Download size={18} />
            Export CSV
          </button>
        </div>

        {/* Attendance Grid */}
        <div
          style={{
            backgroundColor: colors.white,
            borderRadius: "1rem",
            padding: "1.5rem",
            boxShadow: "0 1px 3px rgba(0, 0, 0, 0.1)",
            overflowX: "auto",
          }}
        >
          {/* Week Navigation */}
          <div
            style={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              gap: "1rem",
              marginBottom: "1.5rem",
            }}
          >
            <button
              onClick={() => navigateWeek("prev")}
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                padding: "0.5rem",
                borderRadius: "0.5rem",
                border: `1px solid ${colors.border}`,
                backgroundColor: colors.white,
                cursor: "pointer",
                transition: "all 0.2s",
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

            <span
              style={{
                fontSize: "1rem",
                fontWeight: "600",
                color: colors.text.primary,
              }}
            >
              Week View
            </span>

            <button
              onClick={() => navigateWeek("next")}
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                padding: "0.5rem",
                borderRadius: "0.5rem",
                border: `1px solid ${colors.border}`,
                backgroundColor: colors.white,
                cursor: "pointer",
                transition: "all 0.2s",
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
          <div style={{ minWidth: "800px" }}>
            <table
              style={{
                width: "100%",
                borderCollapse: "separate",
                borderSpacing: 0,
              }}
            >
              <thead>
                <tr>
                  {/* Student Name Column Header */}
                  <th
                    style={{
                      padding: "1rem",
                      textAlign: "left",
                      fontSize: "0.875rem",
                      fontWeight: "600",
                      color: colors.text.primary,
                      backgroundColor: colors.background,
                      borderTopLeftRadius: "0.5rem",
                      border: `1px solid ${colors.border}`,
                      borderRight: "none",
                    }}
                  >
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
                          padding: "1rem",
                          textAlign: "center",
                          fontSize: "0.875rem",
                          fontWeight: "600",
                          color: isTodayDate
                            ? colors.primary
                            : colors.text.primary,
                          backgroundColor: isTodayDate
                            ? colors.currentDay
                            : colors.background,
                          border: `1px solid ${colors.border}`,
                          borderLeft: "none",
                          borderRight:
                            index === weekDates.length - 1
                              ? `1px solid ${colors.border}`
                              : "none",
                          borderTopRightRadius:
                            index === weekDates.length - 1 ? "0.5rem" : 0,
                          cursor: "pointer",
                          transition: "all 0.2s",
                        }}
                        onMouseEnter={(e) => {
                          if (!isTodayDate) {
                            e.currentTarget.style.backgroundColor =
                              colors.currentDay;
                          }
                        }}
                        onMouseLeave={(e) => {
                          if (!isTodayDate) {
                            e.currentTarget.style.backgroundColor =
                              colors.background;
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
                {filteredStudents.map((student, studentIndex) => (
                  <tr key={student.id}>
                    {/* Student Name Cell */}
                    <td
                      style={{
                        padding: "1rem",
                        fontSize: "0.875rem",
                        color: colors.text.primary,
                        backgroundColor: colors.white,
                        border: `1px solid ${colors.border}`,
                        borderTop: "none",
                        borderRight: "none",
                        borderBottomLeftRadius:
                          studentIndex === filteredStudents.length - 1
                            ? "0.5rem"
                            : 0,
                      }}
                    >
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
                            padding: "1rem",
                            textAlign: "center",
                            backgroundColor: isTodayDate
                              ? colors.currentDay
                              : colors.white,
                            border: `1px solid ${colors.border}`,
                            borderTop: "none",
                            borderLeft: "none",
                            borderRight:
                              dateIndex === weekDates.length - 1
                                ? `1px solid ${colors.border}`
                                : "none",
                            borderBottomRightRadius:
                              studentIndex === filteredStudents.length - 1 &&
                              dateIndex === weekDates.length - 1
                                ? "0.5rem"
                                : 0,
                          }}
                        >
                          <input
                            type="checkbox"
                            checked={present}
                            onChange={() => toggleAttendance(student.id, date)}
                            style={{
                              width: "18px",
                              height: "18px",
                              cursor: "pointer",
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
          <div
            style={{
              marginTop: "1.5rem",
              padding: "1rem",
              backgroundColor: colors.background,
              borderRadius: "0.5rem",
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              flexWrap: "wrap",
              gap: "1rem",
            }}
          >
            <div
              style={{
                fontSize: "0.875rem",
                color: colors.text.secondary,
              }}
            >
              <strong>{filteredStudents.length}</strong> students in{" "}
              {mockGroups.find((g) => g.id === selectedGroupId)?.name}
            </div>
            <div
              style={{
                fontSize: "0.75rem",
                color: colors.text.light,
              }}
            >
              Click day headers to toggle entire day • Click checkboxes for
              individual attendance
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ClassAttendanceScreen;

import { ChevronLeft, ChevronRight, Plus } from "lucide-react";
import React, { useState, useMemo } from "react";

// TypeScript Interfaces
interface Group {
  id: string;
  name: string;
  color: string;
}

interface Class {
  id: string;
  name: string;
  groupId: string;
  dayOfWeek: number; // 0 = Monday, 6 = Sunday
  startHour: number; // 7-21 (7 AM - 9 PM)
  duration: number; // in hours (can be decimal like 1.5)
  color: string;
}

// Mock Data
const mockGroups: Group[] = [
  { id: "1", name: "Yoga Basics", color: "#10B981" },
  { id: "2", name: "Advanced Fitness", color: "#3B82F6" },
  { id: "3", name: "Dance Classes", color: "#F59E0B" },
  { id: "4", name: "Martial Arts", color: "#EF4444" },
  { id: "5", name: "Pilates", color: "#8B5CF6" },
];

const mockClasses: Class[] = [
  {
    id: "1",
    name: "Morning Yoga",
    groupId: "1",
    dayOfWeek: 0,
    startHour: 8,
    duration: 1.5,
    color: "#10B981",
  },
  {
    id: "2",
    name: "Cardio Blast",
    groupId: "2",
    dayOfWeek: 0,
    startHour: 10,
    duration: 1,
    color: "#3B82F6",
  },
  {
    id: "3",
    name: "Evening Flow",
    groupId: "1",
    dayOfWeek: 0,
    startHour: 18,
    duration: 1,
    color: "#10B981",
  },

  {
    id: "4",
    name: "HIIT Training",
    groupId: "2",
    dayOfWeek: 1,
    startHour: 9,
    duration: 1,
    color: "#3B82F6",
  },
  {
    id: "5",
    name: "Ballet Basics",
    groupId: "3",
    dayOfWeek: 1,
    startHour: 14,
    duration: 2,
    color: "#F59E0B",
  },
  {
    id: "6",
    name: "Karate Kids",
    groupId: "4",
    dayOfWeek: 1,
    startHour: 16,
    duration: 1,
    color: "#EF4444",
  },

  {
    id: "7",
    name: "Pilates Core",
    groupId: "5",
    dayOfWeek: 2,
    startHour: 7,
    duration: 1,
    color: "#8B5CF6",
  },
  {
    id: "8",
    name: "Strength",
    groupId: "2",
    dayOfWeek: 2,
    startHour: 11,
    duration: 1.5,
    color: "#3B82F6",
  },
  {
    id: "9",
    name: "Hip Hop",
    groupId: "3",
    dayOfWeek: 2,
    startHour: 17,
    duration: 1.5,
    color: "#F59E0B",
  },

  {
    id: "10",
    name: "Yoga Flow",
    groupId: "1",
    dayOfWeek: 3,
    startHour: 8,
    duration: 1,
    color: "#10B981",
  },
  {
    id: "11",
    name: "Boxing",
    groupId: "4",
    dayOfWeek: 3,
    startHour: 12,
    duration: 1,
    color: "#EF4444",
  },
  {
    id: "12",
    name: "Reformer",
    groupId: "5",
    dayOfWeek: 3,
    startHour: 15,
    duration: 1.5,
    color: "#8B5CF6",
  },

  {
    id: "13",
    name: "CrossFit",
    groupId: "2",
    dayOfWeek: 4,
    startHour: 7,
    duration: 1,
    color: "#3B82F6",
  },
  {
    id: "14",
    name: "Contemporary",
    groupId: "3",
    dayOfWeek: 4,
    startHour: 13,
    duration: 2,
    color: "#F59E0B",
  },
  {
    id: "15",
    name: "Tai Chi",
    groupId: "4",
    dayOfWeek: 4,
    startHour: 19,
    duration: 1,
    color: "#EF4444",
  },

  {
    id: "16",
    name: "Power Yoga",
    groupId: "1",
    dayOfWeek: 5,
    startHour: 9,
    duration: 1.5,
    color: "#10B981",
  },
  {
    id: "17",
    name: "Spin Class",
    groupId: "2",
    dayOfWeek: 5,
    startHour: 11,
    duration: 1,
    color: "#3B82F6",
  },
  {
    id: "18",
    name: "Mat Pilates",
    groupId: "5",
    dayOfWeek: 5,
    startHour: 16,
    duration: 1,
    color: "#8B5CF6",
  },

  {
    id: "19",
    name: "Salsa",
    groupId: "3",
    dayOfWeek: 6,
    startHour: 10,
    duration: 2,
    color: "#F59E0B",
  },
  {
    id: "20",
    name: "Kickboxing",
    groupId: "4",
    dayOfWeek: 6,
    startHour: 14,
    duration: 1,
    color: "#EF4444",
  },
  {
    id: "21",
    name: "Restorative",
    groupId: "1",
    dayOfWeek: 6,
    startHour: 18,
    duration: 1.5,
    color: "#10B981",
  },
];

const AttendanceCalendar: React.FC = () => {
  // State Management
  const [currentWeekStart, setCurrentWeekStart] = useState<Date>(() => {
    const today = new Date();
    const dayOfWeek = today.getDay();
    const diff = dayOfWeek === 0 ? -6 : 1 - dayOfWeek; // Adjust to Monday
    const monday = new Date(today);
    monday.setDate(today.getDate() + diff);
    monday.setHours(0, 0, 0, 0);
    return monday;
  });

  const [selectedGroupId, setSelectedGroupId] = useState<string>("all");

  // Constants
  const DAYS = [
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
    "Sunday",
  ];
  const HOURS = Array.from({ length: 14 }, (_, i) => i + 7); // 7 AM to 9 PM
  const HOUR_HEIGHT = 60; // pixels per hour

  // Utility Functions
  const formatWeekRange = (startDate: Date): string => {
    const endDate = new Date(startDate);
    endDate.setDate(startDate.getDate() + 6);

    const options: Intl.DateTimeFormatOptions = {
      month: "short",
      day: "numeric",
    };
    const start = startDate.toLocaleDateString("en-US", options);
    const end = endDate.toLocaleDateString("en-US", options);

    return `${start} - ${end}`;
  };

  const navigateWeek = (direction: "prev" | "next") => {
    const newDate = new Date(currentWeekStart);
    newDate.setDate(
      currentWeekStart.getDate() + (direction === "next" ? 7 : -7),
    );
    setCurrentWeekStart(newDate);
  };

  const handleAddGroup = () => {
    console.warn("Navigate to Add Group screen");
    // Navigation logic would go here
  };

  const handleAddClass = () => {
    console.warn("Navigate to Add Class screen");
    // Navigation logic would go here
  };

  // Filter classes by selected group
  const filteredClasses = useMemo(() => {
    if (selectedGroupId === "all") return mockClasses;
    return mockClasses.filter((cls) => cls.groupId === selectedGroupId);
  }, [selectedGroupId]);

  // Format time display
  const formatTime = (hour: number): string => {
    const period = hour >= 12 ? "PM" : "AM";
    const displayHour = hour > 12 ? hour - 12 : hour === 0 ? 12 : hour;
    return `${displayHour} ${period}`;
  };

  // Theme colors
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
        {/* Title */}
        <h1
          style={{
            fontSize: "2rem",
            fontWeight: "bold",
            color: colors.text.primary,
            marginBottom: "2rem",
            textAlign: "center",
          }}
        >
          Attendance Calendar
        </h1>

        {/* Calendar Controls */}
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
          {/* Left Section - Group Selection */}
          <div
            style={{ display: "flex", alignItems: "center", gap: "0.75rem" }}
          >
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
              }}
              onMouseEnter={(e) =>
                (e.currentTarget.style.borderColor = colors.primary)
              }
              onMouseLeave={(e) =>
                (e.currentTarget.style.borderColor = colors.border)
              }
            >
              <option value="all">All Groups</option>
              {mockGroups.map((group) => (
                <option key={group.id} value={group.id}>
                  {group.name}
                </option>
              ))}
            </select>

            <button
              onClick={handleAddGroup}
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                padding: "0.625rem",
                borderRadius: "0.5rem",
                border: "none",
                backgroundColor: colors.primary,
                color: colors.white,
                cursor: "pointer",
                transition: "all 0.2s",
              }}
              onMouseEnter={(e) =>
                (e.currentTarget.style.backgroundColor = colors.primaryHover)
              }
              onMouseLeave={(e) =>
                (e.currentTarget.style.backgroundColor = colors.primary)
              }
              onMouseDown={(e) =>
                (e.currentTarget.style.transform = "scale(0.95)")
              }
              onMouseUp={(e) => (e.currentTarget.style.transform = "scale(1)")}
            >
              <Plus size={20} />
            </button>
          </div>

          {/* Center Section - Week Navigation */}
          <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
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
                minWidth: "160px",
                textAlign: "center",
              }}
            >
              {formatWeekRange(currentWeekStart)}
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

          {/* Right Section - Add Class */}
          <button
            onClick={handleAddClass}
            style={{
              display: "flex",
              alignItems: "center",
              gap: "0.5rem",
              padding: "0.625rem 1.25rem",
              borderRadius: "0.5rem",
              border: "none",
              backgroundColor: colors.primary,
              color: colors.white,
              fontSize: "0.875rem",
              fontWeight: "500",
              cursor: "pointer",
              transition: "all 0.2s",
            }}
            onMouseEnter={(e) =>
              (e.currentTarget.style.backgroundColor = colors.primaryHover)
            }
            onMouseLeave={(e) =>
              (e.currentTarget.style.backgroundColor = colors.primary)
            }
            onMouseDown={(e) =>
              (e.currentTarget.style.transform = "scale(0.95)")
            }
            onMouseUp={(e) => (e.currentTarget.style.transform = "scale(1)")}
          >
            <Plus size={18} />
            Add Class
          </button>
        </div>

        {/* Calendar Grid */}
        <div
          style={{
            backgroundColor: colors.white,
            borderRadius: "1rem",
            padding: "1.5rem",
            boxShadow: "0 1px 3px rgba(0, 0, 0, 0.1)",
            overflowX: "auto",
          }}
        >
          <div style={{ display: "flex", gap: "0.5rem", minWidth: "900px" }}>
            {/* Time Labels Column */}
            <div style={{ width: "80px", flexShrink: 0 }}>
              {/* Empty space for day names row */}
              <div style={{ height: "60px", marginBottom: "0.5rem" }} />

              {/* Hour Labels */}
              {HOURS.map((hour) => (
                <div
                  key={hour}
                  style={{
                    height: `${HOUR_HEIGHT}px`,
                    display: "flex",
                    alignItems: "flex-start",
                    justifyContent: "flex-end",
                    paddingRight: "0.75rem",
                    fontSize: "0.75rem",
                    color: colors.text.secondary,
                    fontWeight: "500",
                  }}
                >
                  {formatTime(hour)}
                </div>
              ))}
            </div>

            {/* Calendar Days Grid */}
            <div style={{ flex: 1, display: "flex", gap: "0.5rem" }}>
              {DAYS.map((day, dayIndex) => (
                <div
                  key={day}
                  style={{
                    flex: 1,
                    minWidth: "120px",
                  }}
                >
                  {/* Day Name Header */}
                  <div
                    style={{
                      height: "60px",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      fontSize: "0.875rem",
                      fontWeight: "600",
                      color: colors.text.primary,
                      backgroundColor: colors.background,
                      borderRadius: "0.5rem",
                      marginBottom: "0.5rem",
                    }}
                  >
                    {day}
                  </div>

                  {/* Day Column with Hours */}
                  <div
                    style={{
                      position: "relative",
                      height: `${HOURS.length * HOUR_HEIGHT}px`,
                      border: `1px solid ${colors.border}`,
                      borderRadius: "0.5rem",
                      overflow: "hidden",
                    }}
                  >
                    {/* Hour Grid Lines */}
                    {HOURS.map((hour, hourIndex) => (
                      <div
                        key={hour}
                        style={{
                          position: "absolute",
                          top: `${hourIndex * HOUR_HEIGHT}px`,
                          left: 0,
                          right: 0,
                          height: `${HOUR_HEIGHT}px`,
                          borderBottom:
                            hourIndex < HOURS.length - 1
                              ? `1px solid ${colors.border}`
                              : "none",
                        }}
                      />
                    ))}

                    {/* Classes for this day */}
                    {filteredClasses
                      .filter((cls) => cls.dayOfWeek === dayIndex)
                      .map((cls) => {
                        const topPosition = (cls.startHour - 7) * HOUR_HEIGHT;
                        const height = cls.duration * HOUR_HEIGHT;

                        return (
                          <div
                            key={cls.id}
                            style={{
                              position: "absolute",
                              top: `${topPosition}px`,
                              left: "4px",
                              right: "4px",
                              height: `${height - 8}px`,
                              backgroundColor: cls.color,
                              borderRadius: "0.375rem",
                              padding: "0.5rem",
                              color: colors.white,
                              fontSize: "0.75rem",
                              fontWeight: "600",
                              overflow: "hidden",
                              cursor: "pointer",
                              transition: "all 0.2s",
                              boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)",
                            }}
                            onMouseEnter={(e) => {
                              e.currentTarget.style.transform = "scale(1.02)";
                              e.currentTarget.style.boxShadow =
                                "0 4px 8px rgba(0, 0, 0, 0.2)";
                              e.currentTarget.style.zIndex = "10";
                            }}
                            onMouseLeave={(e) => {
                              e.currentTarget.style.transform = "scale(1)";
                              e.currentTarget.style.boxShadow =
                                "0 2px 4px rgba(0, 0, 0, 0.1)";
                              e.currentTarget.style.zIndex = "1";
                            }}
                          >
                            <div
                              style={{
                                fontWeight: "700",
                                marginBottom: "0.125rem",
                              }}
                            >
                              {cls.name}
                            </div>
                            <div style={{ fontSize: "0.625rem", opacity: 0.9 }}>
                              {formatTime(cls.startHour)} -{" "}
                              {formatTime(cls.startHour + cls.duration)}
                            </div>
                          </div>
                        );
                      })}
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AttendanceCalendar;

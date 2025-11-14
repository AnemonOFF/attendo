/* eslint-disable sonarjs/no-duplicate-string */
import { ArrowLeft, Plus, Trash2 } from "lucide-react";
import React, { useState, useRef, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";

// TypeScript Interfaces
interface Student {
  id: string;
  name: string;
  email: string;
  groupId: string;
  joinDate: string;
}

interface SwipeState {
  studentId: string;
  offset: number;
  isDragging: boolean;
  startX: number;
}

const mockStudents: Student[] = [
  {
    id: "1",
    name: "Alice Johnson",
    email: "alice.johnson@email.com",
    groupId: "1",
    joinDate: "2024-01-15",
  },
  {
    id: "2",
    name: "Bob Smith",
    email: "bob.smith@email.com",
    groupId: "1",
    joinDate: "2024-01-20",
  },
  {
    id: "3",
    name: "Charlie Brown",
    email: "charlie.brown@email.com",
    groupId: "1",
    joinDate: "2024-02-05",
  },
  {
    id: "4",
    name: "Diana Prince",
    email: "diana.prince@email.com",
    groupId: "1",
    joinDate: "2024-02-12",
  },
  {
    id: "5",
    name: "Ethan Hunt",
    email: "ethan.hunt@email.com",
    groupId: "1",
    joinDate: "2024-02-18",
  },
  {
    id: "6",
    name: "Fiona Apple",
    email: "fiona.apple@email.com",
    groupId: "1",
    joinDate: "2024-03-01",
  },
];

const EditGroupScreen: React.FC = () => {
  // State Management
  const [students, setStudents] = useState<Student[]>(mockStudents);
  const [swipeState, setSwipeState] = useState<SwipeState | null>(null);
  const [deleteConfirmation, setDeleteConfirmation] = useState<{
    show: boolean;
    studentId: string;
    studentName: string;
  }>({
    show: false,
    studentId: "",
    studentName: "",
  });

  const swipeRefs = useRef<{ [key: string]: HTMLDivElement | null }>({});

  const navigate = useNavigate();
  const location = useLocation();
  const { state } = location;

  // Theme colors (matching previous screens)
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
    danger: "#EF4444",
    dangerLight: "#FEE2E2",
    dangerHover: "#DC2626",
  };

  // Navigation handlers
  const handleBackButton = () => {
    navigate(-1);
    // Navigation logic would go here
  };

  const handleAddStudent = () => {
    // Navigation logic would go here
  };

  // Swipe gesture handling
  const handleTouchStart = (e: React.TouchEvent, studentId: string) => {
    const touch = e.touches[0];
    setSwipeState({
      studentId,
      offset: 0,
      isDragging: true,
      startX: touch.clientX,
    });
  };

  const handleMouseDown = (e: React.MouseEvent, studentId: string) => {
    e.preventDefault();
    setSwipeState({
      studentId,
      offset: 0,
      isDragging: true,
      startX: e.clientX,
    });
  };

  const handleTouchMove = (e: React.TouchEvent) => {
    if (!swipeState || !swipeState.isDragging) return;

    const touch = e.touches[0];
    const deltaX = touch.clientX - swipeState.startX;
    const maxOffset = -80; // Maximum swipe distance
    const offset = Math.max(maxOffset, Math.min(0, deltaX));

    setSwipeState({
      ...swipeState,
      offset,
    });

    // Apply transform to the element
    const element = swipeRefs.current[swipeState.studentId];
    if (element) {
      element.style.transform = `translateX(${offset}px)`;
    }
  };

  /*   const handleMouseMove = (e: React.MouseEvent) => {
    if (!swipeState || !swipeState.isDragging) return;
    
    e.preventDefault();
    const deltaX = e.clientX - swipeState.startX;
    const maxOffset = -80;
    const offset = Math.max(maxOffset, Math.min(0, deltaX));
    
    setSwipeState({
      ...swipeState,
      offset
    });

    const element = swipeRefs.current[swipeState.studentId];
    if (element) {
      element.style.transform = `translateX(${offset}px)`;
    }
  }; */

  const handleTouchEnd = () => {
    if (!swipeState) return;

    const element = swipeRefs.current[swipeState.studentId];
    if (element) {
      // If swiped more than halfway, keep it open, otherwise close
      if (swipeState.offset < -40) {
        element.style.transform = "translateX(-80px)";
      } else {
        element.style.transform = "translateX(0px)";
      }
    }

    setSwipeState(null);
  };

  /* const handleMouseUp = () => {
    if (!swipeState) return;
    
    const element = swipeRefs.current[swipeState.studentId];
    if (element) {
      if (swipeState.offset < -40) {
        element.style.transform = 'translateX(-80px)';
      } else {
        element.style.transform = 'translateX(0px)';
      }
    }
    
    setSwipeState(null);
  }; */

  // Global mouse/touch event listeners
  useEffect(() => {
    const handleGlobalMouseMove = (e: MouseEvent) => {
      if (swipeState && swipeState.isDragging) {
        const deltaX = e.clientX - swipeState.startX;
        const maxOffset = -80;
        const offset = Math.max(maxOffset, Math.min(0, deltaX));

        setSwipeState({
          ...swipeState,
          offset,
        });

        const element = swipeRefs.current[swipeState.studentId];
        if (element) {
          element.style.transform = `translateX(${offset}px)`;
        }
      }
    };

    const handleGlobalMouseUp = () => {
      if (swipeState) {
        const element = swipeRefs.current[swipeState.studentId];
        if (element) {
          if (swipeState.offset < -40) {
            element.style.transform = "translateX(-80px)";
          } else {
            element.style.transform = "translateX(0px)";
          }
        }
        setSwipeState(null);
      }
    };

    if (swipeState && swipeState.isDragging) {
      document.addEventListener("mousemove", handleGlobalMouseMove);
      document.addEventListener("mouseup", handleGlobalMouseUp);
    }

    return () => {
      document.removeEventListener("mousemove", handleGlobalMouseMove);
      document.removeEventListener("mouseup", handleGlobalMouseUp);
    };
  }, [swipeState]);

  // Delete confirmation handlers
  const showDeleteConfirmation = (studentId: string, studentName: string) => {
    setDeleteConfirmation({
      show: true,
      studentId,
      studentName,
    });
  };

  const handleDeleteStudent = () => {
    setStudents((prev) =>
      prev.filter((student) => student.id !== deleteConfirmation.studentId),
    );
    setDeleteConfirmation({ show: false, studentId: "", studentName: "" });

    // Reset any open swipe states
    Object.keys(swipeRefs.current).forEach((studentId) => {
      const element = swipeRefs.current[studentId];
      if (element) {
        element.style.transform = "translateX(0px)";
      }
    });
  };

  const cancelDelete = () => {
    setDeleteConfirmation({ show: false, studentId: "", studentName: "" });
  };

  // Format date
  const formatDate = (dateString: string): string => {
    const date = new Date(dateString);
    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        backgroundColor: colors.background,
        padding: "2rem",
      }}
    >
      <div style={{ maxWidth: "800px", margin: "0 auto" }}>
        {/* Top Section - Group Name with Back Button */}
        <div
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            marginBottom: "2rem",
          }}
        >
          <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
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

          {/* Add Student Button */}
          <button
            onClick={handleAddStudent}
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
            Add Student
          </button>
        </div>

        {/* Student List */}
        <div
          style={{
            backgroundColor: colors.white,
            borderRadius: "1rem",
            boxShadow: "0 1px 3px rgba(0, 0, 0, 0.1)",
            overflow: "hidden",
          }}
        >
          {students.length === 0 ? (
            /* Empty State */
            <div
              style={{
                padding: "3rem 2rem",
                textAlign: "center",
              }}
            >
              <div
                style={{
                  width: "4rem",
                  height: "4rem",
                  borderRadius: "50%",
                  backgroundColor: colors.background,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  margin: "0 auto 1rem auto",
                }}
              >
                <Plus size={24} color={colors.text.light} />
              </div>
              <h3
                style={{
                  fontSize: "1.125rem",
                  fontWeight: "600",
                  color: colors.text.primary,
                  margin: "0 0 0.5rem 0",
                }}
              >
                No students yet
              </h3>
              <p
                style={{
                  fontSize: "0.875rem",
                  color: colors.text.secondary,
                  margin: "0 0 1.5rem 0",
                }}
              >
                Add students to this group to get started with attendance
                tracking.
              </p>
              <button
                onClick={handleAddStudent}
                style={{
                  display: "inline-flex",
                  alignItems: "center",
                  gap: "0.5rem",
                  padding: "0.75rem 1.5rem",
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
              >
                <Plus size={18} />
                Add First Student
              </button>
            </div>
          ) : (
            /* Student List */
            <div>
              {/* List Header */}
              <div
                style={{
                  padding: "1rem 1.5rem",
                  borderBottom: `1px solid ${colors.border}`,
                  backgroundColor: colors.background,
                }}
              >
                <div
                  style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                  }}
                >
                  <h2
                    style={{
                      fontSize: "1.125rem",
                      fontWeight: "600",
                      color: colors.text.primary,
                      margin: 0,
                    }}
                  >
                    Students ({students.length})
                  </h2>
                  <p
                    style={{
                      fontSize: "0.75rem",
                      color: colors.text.light,
                      margin: 0,
                    }}
                  >
                    Swipe left to delete
                  </p>
                </div>
              </div>

              {/* Student Rows */}
              <div style={{ position: "relative" }}>
                {students.map((student, index) => (
                  <div
                    key={student.id}
                    style={{
                      position: "relative",
                      borderBottom:
                        index < students.length - 1
                          ? `1px solid ${colors.border}`
                          : "none",
                    }}
                  >
                    {/* Delete Background */}
                    <div
                      style={{
                        position: "absolute",
                        top: 0,
                        right: 0,
                        bottom: 0,
                        width: "80px",
                        backgroundColor: colors.danger,
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                      }}
                    >
                      <button
                        onClick={() =>
                          showDeleteConfirmation(student.id, student.name)
                        }
                        style={{
                          background: "none",
                          border: "none",
                          color: colors.white,
                          cursor: "pointer",
                          padding: "0.5rem",
                          borderRadius: "0.25rem",
                          transition: "all 0.2s",
                        }}
                        onMouseEnter={(e) =>
                          (e.currentTarget.style.backgroundColor =
                            "rgba(255, 255, 255, 0.1)")
                        }
                        onMouseLeave={(e) =>
                          (e.currentTarget.style.backgroundColor =
                            "transparent")
                        }
                      >
                        <Trash2 size={18} />
                      </button>
                    </div>

                    {/* Student Row */}
                    <div
                      ref={(el) => {
                        swipeRefs.current[student.id] = el;
                      }}
                      style={{
                        padding: "1rem 1.5rem",
                        backgroundColor: colors.white,
                        cursor: "grab",
                        transition:
                          swipeState?.studentId === student.id
                            ? "none"
                            : "transform 0.2s ease-out",
                        transform: "translateX(0px)",
                        userSelect: "none",
                      }}
                      onTouchStart={(e) => handleTouchStart(e, student.id)}
                      onTouchMove={handleTouchMove}
                      onTouchEnd={handleTouchEnd}
                      onMouseDown={(e) => handleMouseDown(e, student.id)}
                    >
                      <div
                        style={{
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "space-between",
                        }}
                      >
                        <div style={{ flex: 1 }}>
                          <h3
                            style={{
                              fontSize: "1rem",
                              fontWeight: "600",
                              color: colors.text.primary,
                              margin: "0 0 0.25rem 0",
                            }}
                          >
                            {student.name}
                          </h3>
                          <p
                            style={{
                              fontSize: "0.875rem",
                              color: colors.text.secondary,
                              margin: "0 0 0.25rem 0",
                            }}
                          >
                            {student.email}
                          </p>
                          <p
                            style={{
                              fontSize: "0.75rem",
                              color: colors.text.light,
                              margin: 0,
                            }}
                          >
                            Joined {formatDate(student.joinDate)}
                          </p>
                        </div>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>

        {/* Confirmation Dialog */}
        {deleteConfirmation.show && (
          <div
            style={{
              position: "fixed",
              top: 0,
              left: 0,
              right: 0,
              bottom: 0,
              backgroundColor: "rgba(0, 0, 0, 0.5)",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              zIndex: 1000,
              padding: "2rem",
            }}
          >
            <div
              style={{
                backgroundColor: colors.white,
                borderRadius: "1rem",
                padding: "2rem",
                maxWidth: "400px",
                width: "100%",
                boxShadow: "0 10px 25px rgba(0, 0, 0, 0.2)",
              }}
            >
              <div
                style={{
                  display: "flex",
                  alignItems: "center",
                  gap: "1rem",
                  marginBottom: "1rem",
                }}
              >
                <div
                  style={{
                    width: "3rem",
                    height: "3rem",
                    borderRadius: "50%",
                    backgroundColor: colors.dangerLight,
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                  }}
                >
                  <Trash2 size={20} color={colors.danger} />
                </div>
                <div>
                  <h3
                    style={{
                      fontSize: "1.125rem",
                      fontWeight: "600",
                      color: colors.text.primary,
                      margin: "0 0 0.25rem 0",
                    }}
                  >
                    Remove Student
                  </h3>
                  <p
                    style={{
                      fontSize: "0.875rem",
                      color: colors.text.secondary,
                      margin: 0,
                    }}
                  >
                    Are you sure you want to remove{" "}
                    <strong>{deleteConfirmation.studentName}</strong> from this
                    group?
                  </p>
                </div>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "0.75rem",
                  justifyContent: "flex-end",
                  marginTop: "2rem",
                }}
              >
                <button
                  onClick={cancelDelete}
                  style={{
                    padding: "0.75rem 1.5rem",
                    borderRadius: "0.5rem",
                    border: `1px solid ${colors.border}`,
                    backgroundColor: colors.white,
                    color: colors.text.primary,
                    fontSize: "0.875rem",
                    fontWeight: "500",
                    cursor: "pointer",
                    transition: "all 0.2s",
                  }}
                  onMouseEnter={(e) =>
                    (e.currentTarget.style.backgroundColor = colors.background)
                  }
                  onMouseLeave={(e) =>
                    (e.currentTarget.style.backgroundColor = colors.white)
                  }
                >
                  Cancel
                </button>
                <button
                  onClick={handleDeleteStudent}
                  style={{
                    padding: "0.75rem 1.5rem",
                    borderRadius: "0.5rem",
                    border: "none",
                    backgroundColor: colors.danger,
                    color: colors.white,
                    fontSize: "0.875rem",
                    fontWeight: "500",
                    cursor: "pointer",
                    transition: "all 0.2s",
                  }}
                  onMouseEnter={(e) =>
                    (e.currentTarget.style.backgroundColor = colors.dangerHover)
                  }
                  onMouseLeave={(e) =>
                    (e.currentTarget.style.backgroundColor = colors.danger)
                  }
                >
                  Remove Student
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default EditGroupScreen;

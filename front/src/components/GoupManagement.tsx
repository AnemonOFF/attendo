/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable sonarjs/no-duplicate-string */
import { ArrowLeft } from "lucide-react";
import React, { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";

import { getGroups, createGroup } from "../api/groupController";

// TypeScript interfaces
interface AddGroupModalProps {
  isOpen: boolean;
  onClose: () => void;
  onAddGroup: (groupName: string) => Promise<void>;
  isLoading?: boolean;
  error?: string | null;
}

interface ModalState {
  groupName: string;
  isSubmitting: boolean;
  validationError: string | null;
}

// Main Modal Component
const AddGroupModal: React.FC<AddGroupModalProps> = ({
  isOpen,
  onClose,
  onAddGroup,
  isLoading = false,
  error = null,
}) => {
  const [state, setState] = useState<ModalState>({
    groupName: "",
    isSubmitting: false,
    validationError: null,
  });

  const [isVisible, setIsVisible] = useState(false);
  const [shouldRender, setShouldRender] = useState(false);

  const inputRef = useRef<HTMLInputElement>(null);
  const modalRef = useRef<HTMLDivElement>(null);

  // Theme colors
  const colors = {
    primary: "#3B82F6",
    primaryHover: "#2563EB",
    secondary: "#6B7280",
    success: "#10B981",
    danger: "#EF4444",
    warning: "#F59E0B",
    background: "#F8FAFC",
    overlay: "rgba(0, 0, 0, 0.5)",
    text: {
      primary: "#1F2937",
      secondary: "#6B7280",
      light: "#FFFFFF",
    },
  };

  // Handle modal open/close animations
  useEffect(() => {
    if (isOpen) {
      setShouldRender(true);
      // Start animation after render
      requestAnimationFrame(() => {
        setIsVisible(true);
      });

      // Reset state when modal opens
      setState({
        groupName: "",
        isSubmitting: false,
        validationError: null,
      });

      // Focus input after modal animation
      setTimeout(() => {
        inputRef.current?.focus();
      }, 200);
    } else {
      setIsVisible(false);
      // Remove from DOM after animation
      setTimeout(() => {
        setShouldRender(false);
      }, 200);
    }
  }, [isOpen]);

  // Keyboard event handling
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (!isOpen) return;

      if (e.key === "Escape") {
        onClose();
      } else if (e.key === "Enter" && !state.isSubmitting) {
        handleSubmit();
      }
    };

    if (isOpen) {
      document.addEventListener("keydown", handleKeyDown);
      document.body.style.overflow = "hidden";
    }

    return () => {
      document.removeEventListener("keydown", handleKeyDown);
      document.body.style.overflow = "unset";
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpen, state.isSubmitting]);

  // Form validation
  const validateGroupName = (name: string): string | null => {
    if (!name.trim()) {
      return "Group name is required";
    }
    if (name.trim().length < 2) {
      return "Group name must be at least 2 characters";
    }
    if (name.trim().length > 50) {
      return "Group name must be less than 50 characters";
    }
    return null;
  };

  // Handle input change
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setState((prev) => ({
      ...prev,
      groupName: value,
      validationError: value ? validateGroupName(value) : null,
    }));
  };

  // Handle form submission
  const handleSubmit = async () => {
    const trimmedName = state.groupName.trim();
    const validationError = validateGroupName(trimmedName);

    if (validationError) {
      setState((prev) => ({ ...prev, validationError }));
      inputRef.current?.focus();
      return;
    }

    setState((prev) => ({
      ...prev,
      isSubmitting: true,
      validationError: null,
    }));

    try {
      await onAddGroup(trimmedName);
      onClose();
    } catch {
      // Error handled by parent component
    } finally {
      setState((prev) => ({ ...prev, isSubmitting: false }));
    }
  };

  // Handle overlay click
  const handleOverlayClick = (e: React.MouseEvent) => {
    if (e.target === e.currentTarget && !state.isSubmitting) {
      onClose();
    }
  };

  if (!shouldRender) return null;

  // Animation styles
  const overlayStyle: React.CSSProperties = {
    position: "fixed",
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    zIndex: 50,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    padding: "1rem",
    backgroundColor: isVisible ? colors.overlay : "rgba(0, 0, 0, 0)",
    transition: "background-color 0.2s ease-out",
  };

  const modalStyle: React.CSSProperties = {
    position: "relative",
    width: "100%",
    maxWidth: "28rem",
    backgroundColor: "white",
    borderRadius: "1rem",
    boxShadow: "0 25px 50px -12px rgba(0, 0, 0, 0.25)",
    overflow: "hidden",
    maxHeight: "90vh",
    opacity: isVisible ? 1 : 0,
    transform: isVisible
      ? "translateY(0) scale(1)"
      : "translateY(-20px) scale(0.95)",
    transition: "all 0.2s ease-out",
  };

  return (
    <div style={overlayStyle} onClick={handleOverlayClick}>
      <div
        ref={modalRef}
        style={modalStyle}
        onClick={(e) => e.stopPropagation()}
      >
        {/* Header */}
        <div
          style={{
            padding: "1.25rem 1.5rem",
            borderBottom: "1px solid #F3F4F6",
          }}
        >
          <h2
            style={{
              fontSize: "1.25rem",
              fontWeight: "600",
              color: colors.text.primary,
              margin: 0,
            }}
          >
            Create New Group
          </h2>
          <p
            style={{
              marginTop: "0.25rem",
              fontSize: "0.875rem",
              color: colors.text.secondary,
              margin: "0.25rem 0 0 0",
            }}
          >
            Enter a name for your new group
          </p>
        </div>

        {/* Form Content */}
        <div style={{ padding: "1.5rem" }}>
          {/* Error Display */}
          {error && (
            <div
              style={{
                marginBottom: "1rem",
                padding: "0.75rem",
                borderRadius: "0.5rem",
                border: "1px solid #FECACA",
                backgroundColor: "#FEF2F2",
                color: colors.danger,
              }}
            >
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontSize: "0.875rem" }}>⚠️</span>
                <span style={{ marginLeft: "0.5rem", fontSize: "0.875rem" }}>
                  {error}
                </span>
              </div>
            </div>
          )}

          {/* Input Field */}
          <div style={{ marginBottom: "1.5rem" }}>
            <label
              htmlFor="groupName"
              style={{
                display: "block",
                fontSize: "0.875rem",
                fontWeight: "500",
                marginBottom: "0.5rem",
                color: colors.text.primary,
              }}
            >
              Group Name *
            </label>
            <input
              ref={inputRef}
              id="groupName"
              type="text"
              value={state.groupName}
              onChange={handleInputChange}
              disabled={state.isSubmitting || isLoading}
              style={{
                width: "100%",
                padding: "0.75rem 1rem",
                borderRadius: "0.75rem",
                border: `1px solid ${state.validationError ? colors.danger : "#E5E7EB"}`,
                backgroundColor: colors.background,
                color: colors.text.primary,
                fontSize: "0.875rem",
                outline: "none",
                transition: "all 0.2s",
                boxSizing: "border-box",
              }}
              placeholder="Enter group name..."
              maxLength={50}
              autoComplete="off"
              onFocus={(e) => {
                e.target.style.borderColor = colors.primary;
                e.target.style.boxShadow = `0 0 0 3px ${colors.primary}40`;
              }}
              onBlur={(e) => {
                e.target.style.borderColor = state.validationError
                  ? colors.danger
                  : "#E5E7EB";
                e.target.style.boxShadow = "none";
              }}
            />

            {/* Validation Error */}
            {state.validationError && (
              <p
                style={{
                  marginTop: "0.5rem",
                  fontSize: "0.875rem",
                  color: colors.danger,
                  margin: "0.5rem 0 0 0",
                }}
              >
                {state.validationError}
              </p>
            )}

            {/* Character Counter */}
            <p
              style={{
                marginTop: "0.5rem",
                fontSize: "0.75rem",
                textAlign: "right",
                color: colors.text.secondary,
                margin: "0.5rem 0 0 0",
              }}
            >
              {state.groupName.length}/50
            </p>
          </div>
        </div>

        {/* Footer Actions */}
        <div
          style={{
            padding: "1rem 1.5rem",
            backgroundColor: "#F9FAFB",
            display: "flex",
            justifyContent: "flex-end",
            gap: "0.75rem",
          }}
        >
          <button
            type="button"
            onClick={onClose}
            disabled={state.isSubmitting || isLoading}
            style={{
              padding: "0.5rem 1rem",
              fontSize: "0.875rem",
              fontWeight: "500",
              borderRadius: "0.5rem",
              border: "1px solid #E5E7EB",
              backgroundColor: "white",
              color: colors.text.secondary,
              cursor:
                state.isSubmitting || isLoading ? "not-allowed" : "pointer",
              opacity: state.isSubmitting || isLoading ? 0.5 : 1,
              transition: "all 0.2s",
            }}
            onMouseEnter={(e) => {
              if (!state.isSubmitting && !isLoading) {
                e.currentTarget.style.backgroundColor = "#F3F4F6";
              }
            }}
            onMouseLeave={(e) => {
              if (!state.isSubmitting && !isLoading) {
                e.currentTarget.style.backgroundColor = "white";
              }
            }}
            onMouseDown={(e) => {
              if (!state.isSubmitting && !isLoading) {
                e.currentTarget.style.transform = "scale(0.95)";
              }
            }}
            onMouseUp={(e) => {
              if (!state.isSubmitting && !isLoading) {
                e.currentTarget.style.transform = "scale(1)";
              }
            }}
          >
            Cancel
          </button>
          <button
            type="button"
            onClick={handleSubmit}
            disabled={
              state.isSubmitting ||
              isLoading ||
              !!state.validationError ||
              !state.groupName.trim()
            }
            style={{
              padding: "0.5rem 1.5rem",
              fontSize: "0.875rem",
              fontWeight: "500",
              borderRadius: "0.5rem",
              border: "none",
              backgroundColor: colors.primary,
              color: colors.text.light,
              cursor:
                state.isSubmitting ||
                isLoading ||
                !!state.validationError ||
                !state.groupName.trim()
                  ? "not-allowed"
                  : "pointer",
              opacity:
                state.isSubmitting ||
                isLoading ||
                !!state.validationError ||
                !state.groupName.trim()
                  ? 0.5
                  : 1,
              transition: "all 0.2s",
              display: "flex",
              alignItems: "center",
            }}
            onMouseEnter={(e) => {
              if (
                !state.isSubmitting &&
                !isLoading &&
                !state.validationError &&
                state.groupName.trim()
              ) {
                e.currentTarget.style.opacity = "0.9";
              }
            }}
            onMouseLeave={(e) => {
              if (
                !state.isSubmitting &&
                !isLoading &&
                !state.validationError &&
                state.groupName.trim()
              ) {
                e.currentTarget.style.opacity = "1";
              }
            }}
            onMouseDown={(e) => {
              if (
                !state.isSubmitting &&
                !isLoading &&
                !state.validationError &&
                state.groupName.trim()
              ) {
                e.currentTarget.style.transform = "scale(0.95)";
              }
            }}
            onMouseUp={(e) => {
              if (
                !state.isSubmitting &&
                !isLoading &&
                !state.validationError &&
                state.groupName.trim()
              ) {
                e.currentTarget.style.transform = "scale(1)";
              }
            }}
          >
            {state.isSubmitting || isLoading ? (
              <>
                <div
                  style={{
                    width: "1rem",
                    height: "1rem",
                    border: "2px solid white",
                    borderTop: "2px solid transparent",
                    borderRadius: "50%",
                    animation: "spin 1s linear infinite",
                    marginRight: "0.5rem",
                  }}
                ></div>
                Creating...
              </>
            ) : (
              "Create Group"
            )}
          </button>
        </div>
      </div>

      {/* Inline keyframes for spinner animation */}
      <style>
        {`
          @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
          }
        `}
      </style>
    </div>
  );
};

// Integration Example Component
const GroupManagementDemo = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [groups, setGroups] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Fetch groups from API on mount
  useEffect(() => {
    const fetchGroups = async () => {
      setLoading(true);
      setError(null);
      try {
        const res = await getGroups();
        // API returns AxiosResponse, so use res.data.items
        setGroups(res.data.items.map((g: any) => g.title));
      } catch {
        setError("Failed to load groups.");
      } finally {
        setLoading(false);
      }
    };
    fetchGroups();
  }, []);

  const navigate = useNavigate();
  // const { state } = location;

  // Theme colors matching your existing button
  const colors = {
    primary: "#3B82F6",
    primaryHover: "#2563EB",
    white: "#FFFFFF",
  };

  // API function to add a group
  const handleAddGroup = async (groupName: string): Promise<void> => {
    setLoading(true);
    setError(null);
    try {
      // Call API to create group
      await createGroup({ title: groupName });
      // Refresh group list
      const res = await getGroups();
      setGroups(res.data.items.map((g: any) => g.title));
    } catch (err: any) {
      setError(
        err?.response?.data?.message ||
          err.message ||
          "Failed to create group.",
      );
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const openModal = () => {
    setError(null);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setError(null);
  };

  const handleBackButton = () => {
    navigate(-1);
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        backgroundColor: "#F9FAFB",
        padding: "2rem",
      }}
    >
      <div style={{ maxWidth: "64rem", margin: "0 auto" }}>
        {/* Header */}
        <div style={{ marginBottom: "2rem", textAlign: "center" }}>
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
                (e.currentTarget.style.backgroundColor = "#F9FAFB")
              }
              onMouseLeave={(e) =>
                (e.currentTarget.style.backgroundColor = colors.white)
              }
              onMouseDown={(e) =>
                (e.currentTarget.style.transform = "scale(0.95)")
              }
              onMouseUp={(e) => (e.currentTarget.style.transform = "scale(1)")}
            >
              <ArrowLeft size={20} color="#111827" />
            </button>

            <h1
              style={{
                fontSize: "2rem",
                fontWeight: "bold",
                color: "#111827",
                margin: 0,
              }}
            >
              Group Management
            </h1>
          </div>
        </div>

        {/* Groups List */}
        <div
          style={{
            backgroundColor: "white",
            borderRadius: "1rem",
            boxShadow: "0 10px 15px -3px rgba(0, 0, 0, 0.1)",
            padding: "1.5rem",
          }}
        >
          <h2
            style={{
              fontSize: "1.25rem",
              fontWeight: "600",
              color: "#111827",
              marginBottom: "1rem",
            }}
          >
            Current Groups ({groups.length})
          </h2>
          <div
            style={{
              marginBottom: "2rem",
              display: "flex",
              justifyContent: "center",
            }}
          >
            <button
              onClick={openModal}
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                padding: "0.75rem 1.5rem",
                borderRadius: "0.75rem",
                border: "none",
                backgroundColor: colors.primary,
                color: colors.white,
                cursor: "pointer",
                transition: "all 0.2s",
                fontSize: "0.875rem",
                fontWeight: "500",
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
              <span style={{ marginRight: "0.5rem" }}>+</span>
              Add New Group
            </button>
          </div>

          {groups.length > 0 ? (
            <div style={{ display: "grid", gap: "0.75rem" }}>
              {groups.map((group, index) => (
                <div
                  key={index}
                  style={{
                    display: "flex",
                    alignItems: "center",
                    padding: "1rem",
                    backgroundColor: "#F9FAFB",
                    borderRadius: "0.75rem",
                  }}
                >
                  <div
                    style={{
                      width: "2.5rem",
                      height: "2.5rem",
                      backgroundColor: "#DBEAFE",
                      borderRadius: "50%",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      marginRight: "0.75rem",
                    }}
                  >
                    <span style={{ color: "#2563EB", fontWeight: "600" }}>
                      {group.charAt(0).toUpperCase()}
                    </span>
                  </div>
                  <span style={{ color: "#111827", fontWeight: "500" }}>
                    {group}
                  </span>
                </div>
              ))}
            </div>
          ) : (
            <div
              style={{ textAlign: "center", padding: "2rem", color: "#6B7280" }}
            >
              <p>No groups yet. Create your first group!</p>
            </div>
          )}
        </div>

        {/* Add Group Modal */}
        <AddGroupModal
          isOpen={isModalOpen}
          onClose={closeModal}
          onAddGroup={handleAddGroup}
          isLoading={loading}
          error={error}
        />
      </div>
    </div>
  );
};

export default GroupManagementDemo;

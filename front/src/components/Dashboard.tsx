import React from "react";
import { useNavigate } from "react-router-dom";

const theme = {
  primary: "#4A90E2",
  background: "#F8F9FA",
  white: "#FFFFFF",
  text: {
    primary: "#212529",
    secondary: "#6C757D",
  },
};

const styles = {
  container: {
    minHeight: "100vh",
    background: theme.background,
    padding: "32px",
    display: "flex",
    flexDirection: "column" as const,
    alignItems: "center",
  },
  header: {
    width: "100%",
    maxWidth: "800px",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: "32px",
  },
  logo: {
    color: theme.primary,
    fontWeight: 700,
    fontSize: "32px",
  },
  logoutButton: {
    background: theme.primary,
    color: theme.white,
    border: "none",
    borderRadius: "8px",
    padding: "10px 24px",
    cursor: "pointer",
    fontWeight: 600,
  },
  welcome: {
    fontSize: "22px",
    marginBottom: "24px",
    fontWeight: 400,
    color: theme.text.primary,
  },
  cards: {
    display: "flex",
    gap: "18px",
    flexWrap: "wrap" as const,
  },
  card: {
    background: theme.white,
    borderRadius: "12px",
    boxShadow: "0 1px 4px rgba(0,0,0,0.05)",
    padding: "32px 40px",
    minWidth: "180px",
    textAlign: "center" as const,
  },
  cardTitle: {
    color: theme.text.secondary,
    fontWeight: 500,
    fontSize: "15px",
    marginBottom: "6px",
  },
  cardValue: {
    color: theme.primary,
    fontWeight: 700,
    fontSize: "24px",
  },
};

const Dashboard: React.FC = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.clear();
    navigate("/login");
  };

  return (
    <div style={styles.container}>
      <header style={styles.header}>
        <span style={styles.logo}>Attendo</span>
        <button style={styles.logoutButton} onClick={handleLogout}>
          Logout
        </button>
      </header>
      <div style={styles.welcome}>Welcome to your dashboard!</div>
      <div style={styles.cards}>
        <div style={styles.card}>
          <div style={styles.cardTitle}>Total Classes</div>
          <div style={styles.cardValue}>--</div>
        </div>
        <div style={styles.card}>
          <div style={styles.cardTitle}>Present Today</div>
          <div style={styles.cardValue}>--</div>
        </div>
        <div style={styles.card}>
          <div style={styles.cardTitle}>Attendance Rate</div>
          <div style={styles.cardValue}>--%</div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;

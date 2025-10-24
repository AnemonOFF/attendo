import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";

const SplashScreen: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const timer = setTimeout(() => {
      navigate("/login");
    }, 2000);

    return () => clearTimeout(timer);
  }, [navigate]);

  const styles = {
    container: {
      minHeight: "100vh",
      width: "100%",
      display: "flex",
      flexDirection: "column" as const,
      alignItems: "center",
      justifyContent: "center",
      backgroundColor: "#4A90E2",
      animation: "fadeIn 0.6s ease-in",
      overflow: "hidden",
    },
    logoContainer: {
      textAlign: "center" as const,
      padding: "20px",
    },
    logo: {
      fontSize: "64px",
      fontWeight: "700",
      color: "#FFFFFF",
      letterSpacing: "-1px",
      marginBottom: "16px",
      animation: "fadeInUp 0.8s ease-out",
      fontFamily:
        '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif',
    },
    subtitle: {
      fontSize: "18px",
      fontWeight: "400",
      color: "#FFFFFF",
      opacity: 0.9,
      animation: "fadeInUp 0.8s ease-out 0.2s backwards",
      fontFamily:
        '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif',
    },
  };

  return (
    <>
      <style>{`
        @keyframes fadeIn {
          from {
            opacity: 0;
          }
          to {
            opacity: 1;
          }
        }

        @keyframes fadeInUp {
          from {
            opacity: 0;
            transform: translateY(20px);
          }
          to {
            opacity: 1;
            transform: translateY(0);
          }
        }
      `}</style>

      <div style={styles.container}>
        <div style={styles.logoContainer}>
          <div style={styles.logo}>Attendo</div>
          <div style={styles.subtitle}>Attendance Tracking Made Simple</div>
        </div>
      </div>
    </>
  );
};

export default SplashScreen;

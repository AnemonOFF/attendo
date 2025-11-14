// import React from 'react';
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";

import "./App.css";
// Import auth components
import AddClassScreen from "./components/AddClassScreen";
import AttendanceCalendar from "./components/AttendanceCalendar";
import Login from "./components/auth/Login";
import RecoverPassword from "./components/auth/RecoverPassword";
import Register from "./components/auth/Register";
import SplashScreen from "./components/auth/SplashScreen";
import ClassAttendanceScreen from "./components/ClassAttendanceScreen";
import EditGroupScreen from "./components/EditGroupScreen";
import GroupManagementDemo from "./components/GoupManagement";

function App() {
  return (
    <Router>
      <Routes>
        {/* Auth Routes */}
        <Route path="/" element={<SplashScreen />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/recover" element={<RecoverPassword />} />
        <Route path="*" element={<Navigate to="/" replace />} />
        <Route path="/calendar" element={<AttendanceCalendar />} />
        <Route path="/classInfo" element={<ClassAttendanceScreen />} />
        <Route path="/addClass" element={<AddClassScreen />} />
        <Route path="/editGroup" element={<EditGroupScreen />} />
        <Route path="/addGroup" element={<GroupManagementDemo />} />
      </Routes>
    </Router>
  );
}

export default App;

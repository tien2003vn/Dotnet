import { Routes, Route, useNavigate, Navigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import Login from "./pages/Login";
import Message from "./pages/Message";
import GroupList from "./pages/Group/components/GroupList";
import ProfileGroup from "./pages/ProfileGroup";
import Home from "./pages/Home";
import Information from "./pages/Information";
import DefaultLayout from "./components/Layouts/DefaultLayout";
import Profile from "./pages/Profile";
import NewGroupPage from "./pages/Group/NewGroup";
import Authentication from "./components/Authentication";
import { SetUser } from "./components/Redux/Actions/UserAction";
import LoadingPage from "./pages/Loading/index.jsx";

function App() {
    const dispatch = useDispatch();
    const [loading, setLoading] = useState(true);
    const user = useSelector((state) => state.user.information);

    useEffect(() => {
        getuser();
        
    }, []);

    const getuser = async () => {
        const response = await dispatch(SetUser());
        if (SetUser.fulfilled.match(response)) setLoading(false);

    };

    if (loading) {
        return <LoadingPage />;
    }

    return (
        <Routes>
            <Route element={<DefaultLayout />}>
                <Route
                    path="/message"
                    element={
                        <Authentication>
                            <Message />
                        </Authentication>
                    }
                />
                <Route
                    path="/"
                    element={
                        <Authentication>
                            <Home />
                        </Authentication>
                    }
                />
                <Route
                    path="/group"
                    element={
                        <Authentication>
                            <GroupList />
                        </Authentication>
                    }
                />
                <Route
                    path="/:id"
                    element={
                        <Authentication>
                            <Profile />
                        </Authentication>
                    }
                />
                <Route
                    path="/group/:id"
                    element={
                        <Authentication>
                            <ProfileGroup />
                        </Authentication>
                    }
                />
                <Route
                    path="/information"
                    element={
                        <Authentication>
                            `<Information />
                        </Authentication>
                    }
                />
                <Route
                    path="/new-group"
                    element={
                        <Authentication>
                            <NewGroupPage />
                        </Authentication>
                    }
                />
                {user === null ? (
                    <Route path="/login" element={<Login />} />
                ) : (
                    <Route path="/login" element={<Navigate to="/" />} />
                )}
                <Route path="*" element={<Login />} />
            </Route>
        </Routes>
    );
}

export default App;

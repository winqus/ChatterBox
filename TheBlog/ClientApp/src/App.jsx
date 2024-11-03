import React from 'react';
import { Navigate, Route, Routes } from 'react-router-dom';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Home from './components/organisms/Home/Home';
import Layout from './components/Layout';
import './custom.css';
import LoginForm from './components/molecules/LoginForm/LoginForm';
import RegisterForm from './components/molecules/RegisterForm/RegisterForm';
import ForgotPWForm from './components/molecules/ForgotPWForm/ForgotPWForm';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import UserProfile from './components/organisms/UserProfile/UserProfile';
import PasswordResetForm from './components/molecules/PasswordResetForm/PasswordResetForm';
import CreateArticleForm from './components/molecules/CreateArticleForm/CreateArticleForm';
import UpdateArticleForm from './components/molecules/UpdateArticleForm/UpdateArticleForm';
import RoleManagerMenu from './components/organisms/RoleManagerMenu/RoleManagerMenu';

export default function App () {
  return (
    <Layout>
      <Routes>
        <Route index element={<Home />} />
        <Route path="/account" element={<Navigate to="/account/profile" />} />
        <Route path="/account/login" element={<LoginForm />} />
        <Route path="/account/logout" element={<Navigate to={ApplicationPaths.LogOut} state={{local: true}} />} />
        <Route path="/account/register" element={<RegisterForm />} />
        <Route path="/account/recoverpassword" element={<ForgotPWForm />} />
        <Route path="/account/resetpassword" element={<PasswordResetForm />} />
        <Route path="/account/profile" element={<AuthorizeRoute element={<UserProfile />} />} />
        <Route path="/Identity/*" element={<Navigate to="/" />} />
        <Route path="/Identity/Account/Login" element={<Navigate to="/account/login" />} />
        <Route path="/article/create" element={<AuthorizeRoute element={<CreateArticleForm />} /> } />
        <Route path="/article/edit" element={<AuthorizeRoute element={<UpdateArticleForm />} /> } />
        <Route path="/manage/userroles" element={<AuthorizeRoute element={<RoleManagerMenu />} /> } />

        {ApiAuthorzationRoutes.map((route, index) => {
          const { element, requireAuth, ...rest } = route;
          return <Route key={index} {...rest} element={requireAuth ? <AuthorizeRoute {...rest} element={element} /> : element} />;
        })}
      </Routes>
    </Layout>
  );
}

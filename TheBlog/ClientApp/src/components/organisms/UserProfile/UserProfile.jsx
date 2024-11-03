import { useEffect, useState } from 'react';
import { Form } from 'reactstrap';
import authService from '../../api-authorization/AuthorizeService';
import ActionButton from '../../atoms/ActionButton/ActionButton';
import { useLocation, useNavigate } from 'react-router-dom';
import FormInputPill from '../../atoms/FormInputPill/FormInputPill';
import ErrorList from '../../atoms/ErrorList/ErrorList';

export default function UserProfile() {
  const { state } = useLocation();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState([]);
  const [user, setUser] = useState();

  const getUser = async () => {
    const [user] = await Promise.all([authService.getUser()]);
    setUser(user);
  };
  useEffect(() => {
    getUser();
  }, [state]);

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    const data = new URLSearchParams(new FormData(e.target));
    await fetch(e.target.action, {
      method: "POST",
      body: data,
    })
    .then(async (data) => {
      setIsLoading(false);
      if (data.ok) {
        const txt = await data.text();
        setErrors([txt]);
        setTimeout(() => navigate('/account/profile'), 2000);
      } else {
        setErrors(['Invalid.']);
      }
    })
    .finally(() => {
      setIsLoading(false);
    });
  };

  return (
    <>
      <section className="row justify-content-center">
        <div className="col-lg-6">
          <h1>Your profile, {user?.name}</h1>
          <p className="mb-5">Change your preferred details here.</p>
          <Form action="/api/account/update" method="post" onSubmit={handleFormSubmit}>
            <FormInputPill
              id="emailNew"
              name="NewEmail"
              label="New Email"
              type="email"
            />
            <FormInputPill
              id="usernameNew"
              name="NewUsername"
              label="New Username"
              type="text"
            />
            <FormInputPill
              id="passwordNew"
              name="NewPassword"
              label="New Password"
              type="password"
            />
            <FormInputPill
              id="passwordNewRepeat"
              name="RepeatNewPassword"
              label="Repeat New Password"
              type="password"
            />
            <div className="my-5">
              <FormInputPill
                id="passwordConfirm"
                name="Password"
                label="Current Password"
                type="password"
                required
              />
            </div>
            <ErrorList errors={errors} />
            <ActionButton text="Update" loading={isLoading} args={{type: 'submit'}} />
          </Form>
        </div>
      </section>
    </>
  );
}

import './ForgotPWForm.scss';
import { Form, Modal, ModalBody, ModalHeader } from 'reactstrap';
import FormInputPill from '../../atoms/FormInputPill/FormInputPill';
import FormHeading from '../../atoms/FormHeading/FormHeading';
import FormParagraph from '../../atoms/FormParagraph/FormParagraph';
import ActionButton from '../../atoms/ActionButton/ActionButton';
import { Link } from 'react-router-dom';
import { useState } from 'react';
import ErrorList from '../../atoms/ErrorList/ErrorList';

export default function ForgotPWForm() {
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState([]);
  const [modal, setModal] = useState(null);

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    const data = new URLSearchParams(new FormData(e.target));
    await fetch(e.target.action, {
      method: "POST",
      body: data,
    })
    .then((data) => {
      setIsLoading(false);
      if (data.ok) {
        return data.json();
      } else {
        setErrors(["Invalid."]);
        setModal(null);
      }
    })
    .then((res) => {
      setErrors([res.message]);
      setModal(
        <Modal
          isOpen={true}
          centered={true}
          className="modal-fullscreen-sm-down"
        >
          <ModalHeader>
            Mock Email
          </ModalHeader>
          <ModalBody>
            <h1>Hello, {res.username}!</h1>
            <p>Here's a link to reset password for your account. If you did not request for a new password, please ignore this email</p>
            <a href={res.callbackUrl}>Reset password</a>
          </ModalBody>
        </Modal>
      );
    });
  };

  return (
    <>
      <Form className='passwordForm' action="/api/account/restorepassword" method="post" onSubmit={handleFormSubmit}>
        <FormHeading>Recover Password</FormHeading>
        <FormParagraph>Enter your email to receive a password recovery link.</FormParagraph>
        <FormInputPill
          id="email_1"
          name="Email"
          label="Email"
          type="email"
          required
        />
        <ErrorList errors={errors} />
        <ActionButton text="Send Recovery Link" loading={isLoading} args={{type: 'submit'}} />
        <FormParagraph>
          Remembered? <Link to="/account/login">Log In</Link>
        </FormParagraph>
      </Form>
      {modal ?? undefined}
    </>
  );
}

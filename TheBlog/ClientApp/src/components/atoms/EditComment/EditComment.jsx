import { Button, ButtonGroup, Form, Input } from "reactstrap";

export default function EditComment({
  author,
  datetime,
  text,
  onSubmit,
  onCancel,
  onDelete,
}) {
  return (
    <div className="border border-round m-1 w-100">
      <div className="m-2 mt-0">
        <h2 className="fs-6 mx-1 text-secondary fw-lighter">
          {author} ({datetime})
        </h2>
      </div>
      <Form onSubmit={onSubmit} className="m-1">
        <Input
          id="inputText1"
          name="model.Text"
          type="textarea"
          className="fs-6 p-1"
          defaultValue={text}
        />
        <div className="d-flex justify-content-between m-1">
          <ButtonGroup>
            <Button type="submit" size="sm" color="primary">
              Save
            </Button>
            <Button type="button" size="sm" outline onClick={onCancel}>
              Cancel
            </Button>
          </ButtonGroup>
          <Button type="button" size="sm" color="danger" onClick={onDelete}>
            Delete
          </Button>
        </div>
      </Form>
    </div>
  );
}

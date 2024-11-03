import { Card, CardTitle, Input } from "reactstrap";

export default function ImageSelectInput({ id, name, args}) {
  return (
    <section>
      <Card body className="mt-3">
        <CardTitle tag="h6">Image (optional)</CardTitle>
        <Input
          type="file"
          id={id}
          name={name}
          className="my-1"
          {...args}
        />
      </Card>
    </section>
  );
}
